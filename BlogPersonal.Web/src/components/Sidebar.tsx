import React, { useEffect, useState } from 'react';
import { Link as RouterLink } from 'react-router-dom';
import { isAxiosError } from 'axios';
import axiosInstance from '../api/axiosConfig';
import { API_ENDPOINTS } from '../config/api';
import { useAuth } from '../context/AuthContext';
import {
  Box,
  List,
  ListItem,
  ListItemButton,
  ListItemText,
  Typography,
  Divider,
  Paper,
  ToggleButton,
  ToggleButtonGroup,
  Alert,
  CircularProgress,
  Snackbar
} from '@mui/material';
import {
  Drafts as DraftsIcon,
  Public as PublicIcon,
  Lock as LockIcon,
  Article as ArticleIcon,
  Warning as WarningIcon
} from '@mui/icons-material';

interface Post {
  id: number;
  titulo: string;
  slug: string;
  estadoId: number;
  estado: string;
}

const Sidebar: React.FC = () => {
  const { token, user } = useAuth();
  const [posts, setPosts] = useState<Post[]>([]);
  const [filter, setFilter] = useState<string | null>('all');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [snackbarOpen, setSnackbarOpen] = useState(false);

  useEffect(() => {
    if (!token) return;

    const fetchMyPosts = async () => {
      setLoading(true);
      setError(null);
      try {
        let url = `${API_ENDPOINTS.GET_POSTS}/my-posts`;
        if (filter && filter !== 'all') {
          const statusMap: Record<string, number> = {
            'saved': 1,
            'public': 2,
            'private': 4
          };
          if (statusMap[filter]) {
            url += `?estadoId=${statusMap[filter]}`;
          }
        }

        const response = await axiosInstance.get(url);
        setPosts(response.data);
      } catch (error) {
        console.error('Error fetching my posts:', error);
        let errorMessage = 'Error al cargar tus publicaciones';
        if (isAxiosError(error)) {
          if (error.response?.status === 401) {
            errorMessage = 'Tu sesión ha expirado';
          } else if (error.response?.data?.message) {
            errorMessage = error.response.data.message;
          } else if (error.message === 'Network Error') {
            errorMessage = 'No se pudo conectar con el servidor';
          }
        }
        setError(errorMessage);
        setSnackbarOpen(true);
      } finally {
        setLoading(false);
      }
    };

    fetchMyPosts();
  }, [token, filter]);

  if (!user) return null;

  const handleFilterChange = (
    _event: React.MouseEvent<HTMLElement>,
    newFilter: string | null,
  ) => {
    if (newFilter !== null) {
      setFilter(newFilter);
    }
  };

  const handleCloseSnackbar = () => {
    setSnackbarOpen(false);
  };

  const getStatusIcon = (statusId: number) => {
    switch (statusId) {
      case 1: return <DraftsIcon fontSize="small" color="action" />;
      case 2: return <PublicIcon fontSize="small" color="primary" />;
      case 4: return <LockIcon fontSize="small" color="error" />;
      default: return <ArticleIcon fontSize="small" />;
    }
  };

  return (
    <Paper elevation={0} sx={{ width: 280, borderRight: 1, borderColor: 'divider', height: 'calc(100vh - 64px)', overflowY: 'auto', position: 'sticky', top: 64 }}>
      <Box sx={{ p: 2 }}>
        <Typography variant="h6" gutterBottom fontWeight="bold">
          Mis Publicaciones
        </Typography>
        
        {error && (
          <Alert 
            severity="error" 
            sx={{ mb: 2 }}
            icon={<WarningIcon />}
            onClose={handleCloseSnackbar}
          >
            {error}
          </Alert>
        )}

        {loading && (
          <Box sx={{ display: 'flex', justifyContent: 'center', py: 2 }}>
            <CircularProgress size={30} />
          </Box>
        )}
        
        <ToggleButtonGroup
          value={filter}
          exclusive
          onChange={handleFilterChange}
          aria-label="post filter"
          size="small"
          fullWidth
          sx={{ mb: 2 }}
          disabled={loading}
        >
          <ToggleButton value="all" aria-label="all">
            Todos
          </ToggleButton>
          <ToggleButton value="saved" aria-label="saved">
            <DraftsIcon fontSize="small" />
          </ToggleButton>
          <ToggleButton value="public" aria-label="public">
            <PublicIcon fontSize="small" />
          </ToggleButton>
          <ToggleButton value="private" aria-label="private">
            <LockIcon fontSize="small" />
          </ToggleButton>
        </ToggleButtonGroup>
      </Box>
      <Divider />
      <List>
        {posts.map((post) => (
          <ListItem key={post.id} disablePadding>
            <ListItemButton component={RouterLink} to={`/edit-post/${post.id}`}>
              <Box sx={{ mr: 2, display: 'flex', alignItems: 'center' }}>
                {getStatusIcon(post.estadoId)}
              </Box>
              <ListItemText 
                primary={post.titulo} 
                secondary={post.estado}
                primaryTypographyProps={{ noWrap: true, fontSize: '0.9rem' }}
                secondaryTypographyProps={{ fontSize: '0.75rem' }}
              />
            </ListItemButton>
          </ListItem>
        ))}
        {!loading && posts.length === 0 && (
          <ListItem>
            <ListItemText secondary="No hay publicaciones" sx={{ textAlign: 'center' }} />
          </ListItem>
        )}
      </List>
      
      <Snackbar
        open={snackbarOpen}
        autoHideDuration={6000}
        onClose={handleCloseSnackbar}
        anchorOrigin={{ vertical: 'bottom', horizontal: 'left' }}
      >
        <Alert onClose={handleCloseSnackbar} severity="error" sx={{ width: '100%' }}>
          {error}
        </Alert>
      </Snackbar>
    </Paper>
  );
};

export default Sidebar;
