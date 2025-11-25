import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import axios from 'axios';
import { useAuth } from '../context/AuthContext';
import { 
  Container, 
  Paper, 
  Typography, 
  TextField, 
  Button, 
  Box, 
  Alert,
  Stack,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  type SelectChangeEvent
} from '@mui/material';
import { Save as SaveIcon, Cancel as CancelIcon, Edit as EditIcon } from '@mui/icons-material';
import TagSelector, { type Tag } from '../components/TagSelector';

const EditPostPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { token, user } = useAuth();
  const [formData, setFormData] = useState({
    titulo: '',
    contenido: '',
    resumen: '',
    slug: '',
    idiomaId: 1,
    estadoId: 2,
    permitirComentarios: true,
    fechaPublicacion: new Date().toISOString(),
  });
  const [selectedTags, setSelectedTags] = useState<Tag[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!token) {
      navigate('/login');
      return;
    } else if (user?.rol !== 'Autor' && user?.rol !== 'Administrador') {
      navigate('/');
      return;
    }

    const fetchData = async () => {
      try {
        const postRes = await axios.get(`http://localhost:5141/api/posts/${id}`);
        const post = postRes.data;
        
        setFormData({
          titulo: post.titulo,
          contenido: post.contenido,
          resumen: post.resumen || '',
          slug: post.slug,
          idiomaId: 1, 
          estadoId: post.estadoId || 2, 
          permitirComentarios: true, 
          fechaPublicacion: post.fechaPublicacion || new Date().toISOString()
        });
        
        // Map existing tags
        // The API now returns List<EtiquetaDto> for Etiquetas
        setSelectedTags(post.etiquetas || []);

      } catch (err) {
        console.error('Error fetching data:', err);
        setError('Error al cargar el post.');
      }
    };

    fetchData();
  }, [id, token, navigate, user]);

  const generateSlug = (text: string) => {
    return text
      .toLowerCase()
      .replace(/[^a-z0-9]+/g, '-')
      .replace(/(^-|-$)+/g, '');
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setFormData(prev => {
      const newData = { ...prev, [name]: value };
      if (name === 'titulo') {
        newData.slug = generateSlug(value);
      }
      return newData;
    });
  };

  const handleStatusChange = (e: SelectChangeEvent<number>) => {
    setFormData(prev => ({ ...prev, estadoId: Number(e.target.value) }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      const postData = {
        ...formData,
        categoriaIds: [], 
        etiquetaIds: selectedTags.map(t => t.id)
      };

      await axios.put(`http://localhost:5141/api/posts/${id}`, postData, {
        headers: {
          Authorization: `Bearer ${token}`
        }
      });

      navigate(`/post/${formData.slug}`);
    } catch (err: unknown) {
      console.error('Error updating post:', err);
      setError('Error al actualizar el post. Verifica que tengas permisos.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Container maxWidth="md" sx={{ py: 4 }}>
      <Paper elevation={3} sx={{ p: 4 }}>
        <Box sx={{ mb: 4, display: 'flex', alignItems: 'center', gap: 2 }}>
          <EditIcon color="primary" sx={{ fontSize: 40 }} />
          <Typography variant="h4" component="h1" fontWeight="bold">
            Editar Publicación
          </Typography>
        </Box>

        {error && (
          <Alert severity="error" sx={{ mb: 3 }}>
            {error}
          </Alert>
        )}

        <form onSubmit={handleSubmit}>
          <Stack spacing={3}>
            <TextField
              label="Título"
              name="titulo"
              value={formData.titulo}
              onChange={handleChange}
              required
              fullWidth
              variant="outlined"
            />

            <TextField
              label="Slug (URL amigable)"
              name="slug"
              value={formData.slug}
              onChange={handleChange}
              required
              fullWidth
              variant="outlined"
              helperText="Se genera automáticamente del título"
              InputProps={{
                readOnly: false,
              }}
            />

            <TextField
              label="Resumen"
              name="resumen"
              value={formData.resumen}
              onChange={handleChange}
              required
              fullWidth
              multiline
              rows={3}
              variant="outlined"
              helperText="Breve descripción que aparecerá en el listado."
            />

            <TagSelector 
              selectedTags={selectedTags} 
              onChange={setSelectedTags} 
            />

            <FormControl fullWidth>
              <InputLabel id="estado-label">Estado</InputLabel>
              <Select
                labelId="estado-label"
                value={formData.estadoId}
                label="Estado"
                onChange={handleStatusChange}
              >
                <MenuItem value={1}>Guardado (Borrador)</MenuItem>
                <MenuItem value={2}>Público</MenuItem>
                <MenuItem value={4}>Privado</MenuItem>
              </Select>
            </FormControl>

            <TextField
              label="Contenido"
              name="contenido"
              value={formData.contenido}
              onChange={handleChange}
              required
              fullWidth
              multiline
              rows={10}
              variant="outlined"
              sx={{ fontFamily: 'monospace' }}
            />

            <Box sx={{ display: 'flex', justifyContent: 'flex-end', gap: 2, pt: 2 }}>
              <Button
                variant="outlined"
                color="inherit"
                startIcon={<CancelIcon />}
                onClick={() => navigate('/')}
              >
                Cancelar
              </Button>
              <Button
                type="submit"
                variant="contained"
                color="primary"
                disabled={loading}
                startIcon={<SaveIcon />}
              >
                {loading ? 'Guardando...' : (formData.estadoId === 1 ? 'Guardar Borrador' : 'Actualizar')}
              </Button>
            </Box>
          </Stack>
        </form>
      </Paper>
    </Container>
  );
};

export default EditPostPage;
