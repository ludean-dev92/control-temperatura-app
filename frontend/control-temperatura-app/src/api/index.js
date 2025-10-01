import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5169/api', 
});

export const getFormatos = () => api.get('/formatos');
export const createFormato = (formato) => api.post('/formatos', formato);
