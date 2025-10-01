import { defineStore } from 'pinia';
import { getFormatos, createFormato } from '../api';

export const useFormatoStore = defineStore('formato', {
  state: () => ({
    formatos: [],
  }),
  actions: {
    async fetchFormatos() {
      const res = await getFormatos();
      this.formatos = res.data;
    },
    async addFormato(formato) {
      const res = await createFormato(formato);
      this.formatos.push(res.data);
    },
  },
});
