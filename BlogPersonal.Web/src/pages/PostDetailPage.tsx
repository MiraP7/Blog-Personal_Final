import React, { useEffect, useState, useCallback } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axios from 'axios';
import { useAuth } from '../context/AuthContext';
import {
  Box,
  Typography,
  Paper,
  Chip,
  CircularProgress,
  Container,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle
} from '@mui/material';
import CalendarTodayIcon from '@mui/icons-material/CalendarToday';
import PersonIcon from '@mui/icons-material/Person';
import LocalOfferIcon from '@mui/icons-material/LocalOffer';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import CommentsSection from '../components/CommentsSection';

interface Tag {
  id: number;
  nombre: string;
  slug: string;
}

interface Post {
  id: number;
  titulo: string;
  contenido: string;
  autorNombre: string;
  fechaPublicacion: string;
  categorias: string[];
  etiquetas: Tag[];
}

const PostDetailPage: React.FC = () => {
  const { slug } = useParams<{ slug: string }>();
  const navigate = useNavigate();
  const { user, token } = useAuth();
  const [post, setPost] = useState<Post | null>(null);
  const [comments, setComments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [openDeleteDialog, setOpenDeleteDialog] = useState(false);

  const fetchPostAndComments = useCallback(async () => {
    try {
      const postRes = await axios.get(`http://localhost:5141/api/posts/slug/${slug}`);
      setPost(postRes.data);
      
      if (postRes.data.id) {
        const commentsRes = await axios.get(`http://localhost:5141/api/comments/post/${postRes.data.id}`);
        setComments(commentsRes.data);
      }
    } catch (error) {
      console.error('Error fetching post:', error);
    } finally {
      setLoading(false);
    }
  }, [slug]);

  useEffect(() => {
    fetchPostAndComments();
  }, [fetchPostAndComments]);

  const handleDelete = async () => {
    if (!post || !token) return;
    try {
      await axios.delete(`http://localhost:5141/api/posts/${post.id}`, {
        headers: { Authorization: `Bearer ${token}` }
      });
      navigate('/');
    } catch (error) {
      console.error('Error deleting post:', error);
    }
  };

  if (loading) return (
    <Box sx={{ display: 'flex', justifyContent: 'center', py: 10 }}>
      <CircularProgress />
    </Box>
  );
  
  if (!post) return (
    <Box sx={{ textAlign: 'center', py: 10 }}>
      <Typography variant="h5">Post no encontrado</Typography>
    </Box>
  );

  const canEdit = user && (user.rol === 'Administrador' || user.nombreUsuario === post.autorNombre);

  return (
    <Container maxWidth="md">
      <Paper elevation={3} sx={{ overflow: 'hidden', borderRadius: 2 }}>
        <Box sx={{ p: { xs: 3, md: 5 } }}>
          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
            <Typography variant="h3" component="h1" gutterBottom fontWeight="bold">
              {post.titulo}
            </Typography>
            {canEdit && (
              <Box>
                <Button 
                  startIcon={<EditIcon />} 
                  onClick={() => navigate(`/edit-post/${post.id}`)}
                  sx={{ mr: 1 }}
                >
                  Editar
                </Button>
                <Button 
                  startIcon={<DeleteIcon />} 
                  color="error"
                  onClick={() => setOpenDeleteDialog(true)}
                >
                  Eliminar
                </Button>
              </Box>
            )}
          </Box>
          
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 3, mb: 4, color: 'text.secondary' }}>
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
              <PersonIcon fontSize="small" />
              <Typography variant="body2">{post.autorNombre}</Typography>
            </Box>
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
              <CalendarTodayIcon fontSize="small" />
              <Typography variant="body2">
                {new Date(post.fechaPublicacion).toLocaleDateString()}
              </Typography>
            </Box>
          </Box>

          <Typography variant="body1" paragraph sx={{ whiteSpace: 'pre-line', lineHeight: 1.8, fontSize: '1.1rem' }}>
            {post.contenido}
          </Typography>

          <Box sx={{ mt: 4, pt: 3, borderTop: 1, borderColor: 'divider' }}>
            <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 1 }}>
              {post.categorias.map((cat, idx) => (
                <Chip 
                  key={idx} 
                  label={cat} 
                  color="primary" 
                  variant="outlined" 
                  size="small" 
                />
              ))}
              {post.etiquetas.map((tag, idx) => (
                <Chip 
                  key={idx} 
                  label={tag.nombre} 
                  icon={<LocalOfferIcon />} 
                  size="small" 
                  sx={{ bgcolor: 'action.hover' }}
                />
              ))}
            </Box>
          </Box>
        </Box>
      </Paper>

      <Box sx={{ mt: 4 }}>
        <CommentsSection 
          postId={post.id} 
          comments={comments} 
          onCommentAdded={fetchPostAndComments} 
        />
      </Box>

      <Dialog
        open={openDeleteDialog}
        onClose={() => setOpenDeleteDialog(false)}
      >
        <DialogTitle>{"¿Eliminar publicación?"}</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Esta acción no se puede deshacer. ¿Estás seguro de que quieres eliminar este post?
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setOpenDeleteDialog(false)}>Cancelar</Button>
          <Button onClick={handleDelete} color="error" autoFocus>
            Eliminar
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
};

export default PostDetailPage;
