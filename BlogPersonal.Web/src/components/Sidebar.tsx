import React, { useEffect, useState } from 'react';
import { Link as RouterLink } from 'react-router-dom';
import axios from 'axios';
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
  ToggleButtonGroup
} from '@mui/material';
import {
  Drafts as DraftsIcon,
  Public as PublicIcon,
  Lock as LockIcon,
  Article as ArticleIcon
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

  useEffect(() => {
    if (!token) return;

    const fetchMyPosts = async () => {
      try {
        let url = 'http://localhost:5141/api/posts/my-posts';
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

        const response = await axios.get(url, {
          headers: { Authorization: `Bearer ${token}` }
        });
        setPosts(response.data);
      } catch (error) {
        console.error('Error fetching my posts:', error);
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
        
        <ToggleButtonGroup
          value={filter}
          exclusive
          onChange={handleFilterChange}
          aria-label="post filter"
          size="small"
          fullWidth
          sx={{ mb: 2 }}
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
        {posts.length === 0 && (
          <ListItem>
            <ListItemText secondary="No hay publicaciones" sx={{ textAlign: 'center' }} />
          </ListItem>
        )}
      </List>
    </Paper>
  );
};

export default Sidebar;
