import axios, { AxiosError } from 'axios';
import type { AxiosInstance, InternalAxiosRequestConfig, AxiosResponse } from 'axios';

/**
 * Configuración centralizada de Axios con interceptores
 * Maneja automáticamente errores 401 (token expirado)
 */

// Crear instancia de Axios
export const axiosInstance: AxiosInstance = axios.create({
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

/**
 * Interceptor de respuesta para manejar errores 401
 * Cuando el token expira, redirige a login y limpia el localStorage
 */
axiosInstance.interceptors.response.use(
  (response: AxiosResponse) => {
    // Si la respuesta es exitosa, retornar directamente
    return response;
  },
  (error: AxiosError) => {
    // Verificar si es un error 401 (No Autorizado / Token Expirado)
    if (error.response?.status === 401) {
      console.warn('Token expirado o sesión inválida. Redirigiendo a login...');
      
      // Limpiar datos de sesión
      localStorage.removeItem('authToken');
      localStorage.removeItem('user');
      localStorage.removeItem('userRole');
      
      // Redirigir a página de login
      // Si estamos en el navegador, hacer el redirect
      if (typeof window !== 'undefined') {
        // Mostrar mensaje al usuario
        alert('Tu sesión ha expirado. Por favor, inicia sesión de nuevo.');
        
        // Redirigir a login
        window.location.href = '/login';
      }
    }

    // Para otros errores, simplemente rechazar la promesa
    return Promise.reject(error);
  }
);

/**
 * Interceptor de solicitud para agregar token de autenticación
 * Automáticamente incluye el JWT en el header Authorization
 */
axiosInstance.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    const token = localStorage.getItem('authToken');
    
    if (token) {
      // Agregar token al header Authorization
      config.headers.Authorization = `Bearer ${token}`;
    }
    
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export default axiosInstance;
