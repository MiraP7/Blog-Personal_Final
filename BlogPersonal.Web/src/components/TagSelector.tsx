import React, { useState, useEffect } from 'react';
import axios from 'axios';
import {
  Autocomplete,
  TextField,
  Chip,
  IconButton,
  Box,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  Tooltip
} from '@mui/material';
import { Edit as EditIcon, Delete as DeleteIcon, Add as AddIcon } from '@mui/icons-material';
import { useAuth } from '../context/AuthContext';

export interface Tag {
  id: number;
  nombre: string;
  slug: string;
}

interface TagSelectorProps {
  selectedTags: Tag[];
  onChange: (tags: Tag[]) => void;
}

const TagSelector: React.FC<TagSelectorProps> = ({ selectedTags, onChange }) => {
  const { token } = useAuth();
  const [availableTags, setAvailableTags] = useState<Tag[]>([]);
  const [dialogOpen, setDialogOpen] = useState(false);
  const [editingTag, setEditingTag] = useState<Tag | null>(null);
  const [tagName, setTagName] = useState('');
  const [loading, setLoading] = useState(false);

  const fetchTags = async () => {
    try {
      const response = await axios.get('http://localhost:5141/api/etiquetas');
      setAvailableTags(response.data);
    } catch (error) {
      console.error('Error fetching tags:', error);
    }
  };

  useEffect(() => {
    fetchTags();
  }, []);

  const handleOpenDialog = (tag?: Tag) => {
    if (tag) {
      setEditingTag(tag);
      setTagName(tag.nombre);
    } else {
      setEditingTag(null);
      setTagName('');
    }
    setDialogOpen(true);
  };

  const handleCloseDialog = () => {
    setDialogOpen(false);
    setEditingTag(null);
    setTagName('');
  };

  const handleSaveTag = async () => {
    if (!tagName.trim()) return;
    setLoading(true);
    try {
      if (editingTag) {
        // Update
        await axios.put(
          `http://localhost:5141/api/etiquetas/${editingTag.id}`,
          { id: editingTag.id, nombre: tagName },
          { headers: { Authorization: `Bearer ${token}` } }
        );
      } else {
        // Create
        await axios.post(
          'http://localhost:5141/api/etiquetas',
          { nombre: tagName },
          { headers: { Authorization: `Bearer ${token}` } }
        );
      }
      await fetchTags();
      handleCloseDialog();
    } catch (error) {
      console.error('Error saving tag:', error);
      alert('Error al guardar la etiqueta');
    } finally {
      setLoading(false);
    }
  };

  const handleDeleteTag = async (e: React.MouseEvent, tagId: number) => {
    e.stopPropagation();
    if (!window.confirm('¿Estás seguro de eliminar esta etiqueta?')) return;

    try {
      await axios.delete(`http://localhost:5141/api/etiquetas/${tagId}`, {
        headers: { Authorization: `Bearer ${token}` }
      });
      
      // Remove from selected if present
      const newSelected = selectedTags.filter(t => t.id !== tagId);
      if (newSelected.length !== selectedTags.length) {
        onChange(newSelected);
      }
      
      await fetchTags();
    } catch (error) {
      console.error('Error deleting tag:', error);
      alert('Error al eliminar la etiqueta');
    }
  };

  return (
    <Box>
      <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 1 }}>
        <Autocomplete
          multiple
          fullWidth
          options={availableTags}
          getOptionLabel={(option) => option.nombre}
          value={selectedTags}
          onChange={(_, newValue) => onChange(newValue)}
          filterSelectedOptions
          renderInput={(params) => (
            <TextField
              {...params}
              label="Etiquetas"
              placeholder="Seleccionar etiquetas"
            />
          )}
          renderOption={(props, option) => {
             const { key, ...otherProps } = props;
             return (
              <li key={key} {...otherProps} style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <span>{option.nombre}</span>
                <Box>
                  <Tooltip title="Editar">
                    <IconButton
                      size="small"
                      onClick={(e) => {
                        e.stopPropagation();
                        handleOpenDialog(option);
                      }}
                    >
                      <EditIcon fontSize="small" />
                    </IconButton>
                  </Tooltip>
                  <Tooltip title="Eliminar">
                    <IconButton
                      size="small"
                      color="error"
                      onClick={(e) => handleDeleteTag(e, option.id)}
                    >
                      <DeleteIcon fontSize="small" />
                    </IconButton>
                  </Tooltip>
                </Box>
              </li>
            );
          }}
          renderTags={(value, getTagProps) =>
            value.map((option, index) => {
              const { key, ...tagProps } = getTagProps({ index });
              return (
                <Chip key={key} variant="outlined" label={option.nombre} {...tagProps} />
              );
            })
          }
        />
        <Tooltip title="Crear nueva etiqueta">
          <IconButton onClick={() => handleOpenDialog()} color="primary" sx={{ border: '1px solid', borderRadius: 1 }}>
            <AddIcon />
          </IconButton>
        </Tooltip>
      </Box>

      <Dialog open={dialogOpen} onClose={handleCloseDialog}>
        <DialogTitle>{editingTag ? 'Editar Etiqueta' : 'Nueva Etiqueta'}</DialogTitle>
        <DialogContent>
          <TextField
            autoFocus
            margin="dense"
            label="Nombre de la etiqueta"
            fullWidth
            variant="outlined"
            value={tagName}
            onChange={(e) => setTagName(e.target.value)}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseDialog}>Cancelar</Button>
          <Button onClick={handleSaveTag} variant="contained" disabled={loading}>
            Guardar
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
};

export default TagSelector;
