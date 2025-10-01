import { createRouter, createWebHistory } from 'vue-router';
import FormatoForm from '../views/FormatoForm.vue';
import FormatoList from '../views/FormatoList.vue';

const routes = [
  { path: '/', component: FormatoList },
  { path: '/nuevo', component: FormatoForm },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
