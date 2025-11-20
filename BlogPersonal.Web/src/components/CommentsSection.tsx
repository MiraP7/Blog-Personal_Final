import React, { useState } from 'react';
import { useAuth } from '../context/AuthContext';
import axios from 'axios';
import {
  Box,
  Typography,
  TextField,
  Button,
  Paper,
  Avatar
} from '@mui/material';
import ReplyIcon from '@mui/icons-material/Reply';
import SendIcon from '@mui/icons-material/Send';

interface Comment {
  id: number;
  autorNombre: string;
  contenido: string;
  fechaCreacion: string;
  respuestas: Comment[];
}

interface CommentsSectionProps {
  postId: number;
  comments: Comment[];
  onCommentAdded: () => void;
}

const CommentsSection: React.FC<CommentsSectionProps> = ({ postId, comments, onCommentAdded }) => {
  const { isAuthenticated, token } = useAuth();
  const [newComment, setNewComment] = useState('');
  const [replyTo, setReplyTo] = useState<number | null>(null);

  const handleSubmit = async (e: React.FormEvent, parentId: number | null = null) => {
    e.preventDefault();
    if (!newComment.trim()) return;

    try {
      await axios.post('http://localhost:5141/api/comments', {
        postId,
        contenido: newComment,
        comentarioPadreId: parentId
      }, {
        headers: { Authorization: `Bearer ${token}` }
      });
      setNewComment('');
      setReplyTo(null);
      onCommentAdded();
    } catch (error) {
      console.error('Error posting comment:', error);
    }
  };

  const CommentItem = ({ comment, level = 0 }: { comment: Comment, level?: number }) => (
    <Box sx={{ mb: 2, ml: level * 4 }}>
      <Paper elevation={1} sx={{ p: 2, bgcolor: 'background.paper' }}>
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 1 }}>
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
            <Avatar sx={{ width: 24, height: 24, fontSize: '0.75rem' }}>
              {comment.autorNombre.charAt(0)}
            </Avatar>
            <Typography variant="subtitle2" fontWeight="bold">
              {comment.autorNombre}
            </Typography>
          </Box>
          <Typography variant="caption" color="text.secondary">
            {new Date(comment.fechaCreacion).toLocaleDateString()}
          </Typography>
        </Box>
        
        <Typography variant="body2" sx={{ mb: 1 }}>
          {comment.contenido}
        </Typography>
        
        {isAuthenticated && (
          <Button 
            size="small" 
            startIcon={<ReplyIcon />} 
            onClick={() => setReplyTo(replyTo === comment.id ? null : comment.id)}
            sx={{ textTransform: 'none' }}
          >
            Responder
          </Button>
        )}

        {replyTo === comment.id && (
          <Box component="form" onSubmit={(e) => handleSubmit(e, comment.id)} sx={{ mt: 2, ml: 2 }}>
            <TextField
              fullWidth
              size="small"
              value={newComment}
              onChange={(e) => setNewComment(e.target.value)}
              placeholder="Escribe tu respuesta..."
              multiline
              rows={2}
              sx={{ mb: 1 }}
            />
            <Button 
              type="submit" 
              variant="contained" 
              size="small" 
              endIcon={<SendIcon />}
            >
              Enviar
            </Button>
          </Box>
        )}
      </Paper>

      {comment.respuestas && comment.respuestas.length > 0 && (
        <Box sx={{ mt: 1 }}>
          {comment.respuestas.map(reply => (
            <CommentItem key={reply.id} comment={reply} level={level + 1} />
          ))}
        </Box>
      )}
    </Box>
  );

  return (
    <Box sx={{ mt: 6 }}>
      <Typography variant="h5" gutterBottom fontWeight="bold">
        Comentarios
      </Typography>
      
      {isAuthenticated ? (
        <Box component="form" onSubmit={(e) => handleSubmit(e, null)} sx={{ mb: 4 }}>
          <TextField
            fullWidth
            value={replyTo === null ? newComment : ''}
            onChange={(e) => {
              setReplyTo(null);
              setNewComment(e.target.value);
            }}
            placeholder="Deja un comentario..."
            multiline
            rows={3}
            sx={{ mb: 2 }}
          />
          <Button 
            type="submit" 
            variant="contained" 
            size="large"
            endIcon={<SendIcon />}
          >
            Publicar Comentario
          </Button>
        </Box>
      ) : (
        <Paper sx={{ p: 3, mb: 4, textAlign: 'center', bgcolor: 'action.hover' }}>
          <Typography color="text.secondary">
            Inicia sesión para dejar un comentario.
          </Typography>
        </Paper>
      )}

      <Box>
        {comments.map(comment => (
          <CommentItem key={comment.id} comment={comment} />
        ))}
        {comments.length === 0 && (
          <Typography color="text.secondary" align="center" sx={{ py: 4 }}>
            No hay comentarios aún. ¡Sé el primero!
          </Typography>
        )}
      </Box>
    </Box>
  );
};

export default CommentsSection;
