<template>
  <div>
    <h2>Formatos</h2>
    <ul>
      <li v-for="f in store.formatos" :key="f.formatoId">
        {{ f.destinoId }} - {{ f.fechaProduccion }} - {{ f.realizadoPor }}
      </li>
    </ul>
  </div>
</template>

<script>
import { onMounted } from 'vue';
import { useFormatoStore } from '../stores/formatoStore';

export default {
  setup() {
    const store = useFormatoStore();
    onMounted(() => store.fetchFormatos());
    return { store };
  },
};
</script>
