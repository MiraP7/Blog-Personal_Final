/**
 * Configuración centralizada de URLs de API
 * Este archivo contiene todas las URLs base para comunicarse con el backend
 * Cambiar BASE_URL aquí actualiza todas las peticiones en la aplicación
 */

// Determinar la URL base según el ambiente
export const BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5141';

// Endpoints de autenticación
export const API_ENDPOINTS = {
  // Auth
  LOGIN: `${BASE_URL}/api/auth/login`,
  REGISTER: `${BASE_URL}/api/auth/register`,
  
  // Posts
  GET_POSTS: `${BASE_URL}/api/posts`,
  GET_POST_BY_ID: (id: number) => `${BASE_URL}/api/posts/${id}`,
  CREATE_POST: `${BASE_URL}/api/posts`,
  UPDATE_POST: (id: number) => `${BASE_URL}/api/posts/${id}`,
  DELETE_POST: (id: number) => `${BASE_URL}/api/posts/${id}`,
  
  // Etiquetas (Tags)
  GET_ETIQUETAS: `${BASE_URL}/api/etiquetas`,
  GET_ETIQUETAS_BY_ID: (id: number) => `${BASE_URL}/api/etiquetas/${id}`,
  
  // Comentarios (Comments)
  GET_COMMENTS_BY_POST: (postId: number) => `${BASE_URL}/api/comments/post/${postId}`,
  CREATE_COMMENT: `${BASE_URL}/api/comments`,
  UPDATE_COMMENT: (id: number) => `${BASE_URL}/api/comments/${id}`,
  DELETE_COMMENT: (id: number) => `${BASE_URL}/api/comments/${id}`,
};

export default API_ENDPOINTS;
