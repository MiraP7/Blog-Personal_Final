import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { isAxiosError } from 'axios';
import axiosInstance from '../api/axiosConfig';
import { API_ENDPOINTS } from '../config/api';
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
  type SelectChangeEvent,
  CircularProgress
} from '@mui/material';
import { Save as SaveIcon, Cancel as CancelIcon, Create as CreateIcon } from '@mui/icons-material';
import TagSelector, { type Tag } from '../components/TagSelector';

const CreatePostPage: React.FC = () => {
  const navigate = useNavigate();
  const { token, user } = useAuth();
  const [formData, setFormData] = useState({
    titulo: '',
    contenido: '',
    resumen: '',
    slug: '',
    estadoId: 1 // Default to Guardado (Borrador)
  });
  const [errors, setErrors] = useState<{ [key: string]: string }>({});
  const [selectedTags, setSelectedTags] = useState<Tag[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!token) {
      navigate('/login');
    } else if (user?.rol !== 'Autor' && user?.rol !== 'Administrador') {
      navigate('/');
    }
  }, [token, user, navigate]);

  const generateSlug = (text: string) => {
    return text
      .toLowerCase()
      .replace(/[^a-z0-9]+/g, '-')
      .replace(/(^-|-$)+/g, '');
  };

  // Validación del formulario
  const validateForm = (): boolean => {
    const newErrors: { [key: string]: string } = {};

    if (!formData.titulo.trim()) {
      newErrors.titulo = 'El título es requerido';
    } else if (formData.titulo.length < 5) {
      newErrors.titulo = 'El título debe tener al menos 5 caracteres';
    } else if (formData.titulo.length > 200) {
      newErrors.titulo = 'El título no puede exceder 200 caracteres';
    }

    if (!formData.resumen.trim()) {
      newErrors.resumen = 'El resumen es requerido';
    } else if (formData.resumen.length < 20) {
      newErrors.resumen = 'El resumen debe tener al menos 20 caracteres';
    } else if (formData.resumen.length > 500) {
      newErrors.resumen = 'El resumen no puede exceder 500 caracteres';
    }

    if (!formData.contenido.trim()) {
      newErrors.contenido = 'El contenido es requerido';
    } else if (formData.contenido.length < 50) {
      newErrors.contenido = 'El contenido debe tener al menos 50 caracteres';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
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

    // Limpiar error del campo cuando el usuario empieza a escribir
    if (errors[name]) {
      setErrors({
        ...errors,
        [name]: ''
      });
    }
  };

  const handleStatusChange = (e: SelectChangeEvent<number>) => {
    setFormData(prev => ({ ...prev, estadoId: Number(e.target.value) }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    // Validar formulario antes de enviar
    if (!validateForm()) {
      return;
    }

    setLoading(true);

    try {
      const postData = {
        ...formData,
        idiomaId: 1, // Default to Spanish
        permitirComentarios: true,
        fechaPublicacion: new Date().toISOString(),
        categoriaIds: [], // Fixed property name
        etiquetaIds: selectedTags.map(t => t.id)
      };

      await axiosInstance.post(API_ENDPOINTS.CREATE_POST, postData);

      navigate('/');
    } catch (err: unknown) {
      console.error('Error creating post:', err);
      if (isAxiosError(err)) {
        if (err.response?.status === 401) {
          navigate('/login');
        } else if (err.response?.data?.message) {
          setError(err.response.data.message);
        } else {
          setError('Error al crear el post. Por favor, intenta de nuevo.');
        }
      } else {
        setError('Error inesperado al crear el post.');
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <Container maxWidth="md" sx={{ py: 4 }}>
      <Paper elevation={3} sx={{ p: 4 }}>
        <Box sx={{ mb: 4, display: 'flex', alignItems: 'center', gap: 2 }}>
          <CreateIcon color="primary" sx={{ fontSize: 40 }} />
          <Typography variant="h4" component="h1" fontWeight="bold">
            Crear Nueva Publicación
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
              error={!!errors.titulo}
              helperText={errors.titulo}
              disabled={loading}
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
              disabled={loading}
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
              error={!!errors.resumen}
              disabled={loading}
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
              error={!!errors.contenido}
              helperText={errors.contenido}
              disabled={loading}
            />

            <Box sx={{ display: 'flex', justifyContent: 'flex-end', gap: 2, pt: 2 }}>
              <Button
                variant="outlined"
                color="inherit"
                startIcon={<CancelIcon />}
                onClick={() => navigate('/')}
                disabled={loading}
              >
                Cancelar
              </Button>
              <Button
                type="submit"
                variant="contained"
                color="primary"
                disabled={loading}
                startIcon={loading ? <CircularProgress size={20} /> : <SaveIcon />}
              >
                {loading ? 'Procesando...' : (formData.estadoId === 1 ? 'Guardar Borrador' : 'Publicar')}
              </Button>
            </Box>
          </Stack>
        </form>
      </Paper>
    </Container>
  );
};

export default CreatePostPage;
