import React, { useEffect, useState, useCallback } from 'react';
import axios from 'axios';
import { useSearchParams, Link as RouterLink, useLocation } from 'react-router-dom';
import {
  Card,
  CardContent,
  Typography,
  Box,
  Avatar,
  Chip,
  CircularProgress,
  Alert,
  CardActionArea,
  useTheme
} from '@mui/material';
import VisibilityIcon from '@mui/icons-material/Visibility';
import CommentIcon from '@mui/icons-material/Comment';
import { useAuth } from '../context/AuthContext';

interface Post {
  id: number;
  titulo: string;
  resumen: string;
  autorNombre: string;
  fechaPublicacion: string;
  slug: string;
  vistas: number;
  comentariosCount: number;
  categorias: string[];
}

const HomePage: React.FC = () => {
  const [posts, setPosts] = useState<Post[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [searchParams] = useSearchParams();
  const searchTerm = searchParams.get('search');
  const theme = useTheme();
  const location = useLocation(); // Para detectar navegación
  const { token } = useAuth(); // Obtener token de autenticación

  const fetchPosts = useCallback(async (showLoading = true) => {
    if (showLoading) setLoading(true);
    try {
      const url = searchTerm 
        ? `http://localhost:5141/api/posts?search=${encodeURIComponent(searchTerm)}`
        : 'http://localhost:5141/api/posts';
      
      // Configurar headers incluyendo Authorization si hay token
      const headers: Record<string, string> = {
        'Cache-Control': 'no-cache',
        'Pragma': 'no-cache'
      };
      
      if (token) {
        headers['Authorization'] = `Bearer ${token}`;
      }
        
      const response = await axios.get(url, { headers });
      const data = response.data;
      const items = Array.isArray(data) ? data : (data.items || []);

      setPosts(items);
      setError(null);
    } catch (err: unknown) {
      console.error('Error fetching posts:', err);
      setError('No se pudieron cargar los posts. Asegúrate de que la API esté corriendo.');
    } finally {
      setLoading(false);
    }
  }, [searchTerm, token]);

  // Cargar al montar y cuando cambia searchTerm o navegación
  useEffect(() => {
    fetchPosts();
  }, [fetchPosts, location.key]); // location.key cambia en cada navegación

  // Refrescar cuando la ventana recupera el foco (usuario regresa de otro tab/página)
  useEffect(() => {
    const handleFocus = () => {
      fetchPosts(false); // Sin mostrar loading para que sea más fluido
    };

    window.addEventListener('focus', handleFocus);
    
    // También refrescar cuando el usuario navega de regreso
    const handleVisibilityChange = () => {
      if (document.visibilityState === 'visible') {
        fetchPosts(false);
      }
    };
    
    document.addEventListener('visibilitychange', handleVisibilityChange);

    return () => {
      window.removeEventListener('focus', handleFocus);
      document.removeEventListener('visibilitychange', handleVisibilityChange);
    };
  }, [fetchPosts]);

  if (loading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', py: 10 }}>
        <CircularProgress />
      </Box>
    );
  }

  return (
    <Box>
      {/* Search Results Header */}
      {searchTerm && (
        <Box sx={{ mb: 4, textAlign: 'center' }}>
          <Typography variant="h4" gutterBottom>
            Resultados para "{searchTerm}"
          </Typography>
        </Box>
      )}

      {error && (
        <Alert severity="error" sx={{ mb: 4 }}>
          {error}
        </Alert>
      )}

      {/* Posts Grid using CSS Grid */}
      <Box
        sx={{
          display: 'grid',
          gridTemplateColumns: {
            xs: '1fr',
            md: 'repeat(2, 1fr)',
            lg: 'repeat(3, 1fr)'
          },
          gap: 3
        }}
      >
        {posts.map((post) => (
          <Card
            key={post.id}
            sx={{
              height: '100%',
              display: 'flex',
              flexDirection: 'column',
              transition: 'transform 0.2s, box-shadow 0.2s',
              '&:hover': {
                transform: 'translateY(-4px)',
                boxShadow: `0 10px 20px -5px ${theme.palette.primary.main}40`,
                borderColor: theme.palette.primary.main
              }
            }}
          >
            <CardActionArea component={RouterLink} to={`/post/${post.slug}`} sx={{ flexGrow: 1, display: 'flex', flexDirection: 'column', alignItems: 'stretch' }}>
              {/* Gradient Header Image Placeholder */}
              <Box
                sx={{
                  height: 140,
                  background: `linear-gradient(135deg, ${theme.palette.primary.main}, ${theme.palette.secondary.main})`,
                  position: 'relative'
                }}
              />
              
              <CardContent sx={{ flexGrow: 1, p: 3 }}>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                  <Chip 
                    label={post.categorias && post.categorias.length > 0 ? post.categorias[0] : 'General'} 
                    size="small" 
                    sx={{ 
                      bgcolor: `${theme.palette.primary.main}20`, 
                      color: theme.palette.primary.light,
                      fontWeight: 600
                    }} 
                  />
                  <Typography variant="caption" color="text.secondary">
                    {new Date(post.fechaPublicacion).toLocaleDateString()}
                  </Typography>
                </Box>

                <Typography variant="h6" component="h2" gutterBottom fontWeight="bold" sx={{ lineHeight: 1.3, mb: 1 }}>
                  {post.titulo}
                </Typography>

                <Typography variant="body2" color="text.secondary" sx={{ mb: 2, display: '-webkit-box', WebkitLineClamp: 3, WebkitBoxOrient: 'vertical', overflow: 'hidden' }}>
                  {post.resumen}
                </Typography>

                <Box sx={{ mt: 'auto', pt: 2, borderTop: `1px solid ${theme.palette.divider}`, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                  <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                    <Avatar sx={{ width: 24, height: 24, fontSize: '0.75rem' }}>
                      {post.autorNombre.charAt(0)}
                    </Avatar>
                    <Typography variant="caption" color="text.secondary">
                      {post.autorNombre}
                    </Typography>
                  </Box>
                  
                  <Box sx={{ display: 'flex', gap: 2 }}>
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
                      <VisibilityIcon sx={{ fontSize: 14, color: 'text.secondary' }} />
                      <Typography variant="caption" color="text.secondary">{post.vistas}</Typography>
                    </Box>
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
                      <CommentIcon sx={{ fontSize: 14, color: 'text.secondary' }} />
                      <Typography variant="caption" color="text.secondary">{post.comentariosCount}</Typography>
                    </Box>
                  </Box>
                </Box>
              </CardContent>
            </CardActionArea>
          </Card>
        ))}
      </Box>

      {posts.length === 0 && !loading && !error && (
        <Box sx={{ textAlign: 'center', py: 10 }}>
          <Typography color="text.secondary">
            No se encontraron publicaciones.
          </Typography>
        </Box>
      )}
    </Box>
  );
};

export default HomePage;
