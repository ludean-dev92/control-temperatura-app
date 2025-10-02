import { createRouter, createWebHistory } from 'vue-router';
import FormatoForm from '../views/FormatoForm.vue';
import FormatoList from '../views/FormatoList.vue';
import Reporte from '../views/Reporte.vue';

const routes = [
  { path: '/', component: FormatoList },
  { path: '/nuevo', component: FormatoForm },
  { path: '/reporte', name: 'Reporte', component: Reporte },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
