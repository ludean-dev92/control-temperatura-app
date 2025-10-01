<template>
  <div>
    <h2>Nuevo Formato</h2>
    <form @submit.prevent="submit">
      <input v-model="formato.destinoId" placeholder="Destino ID" type="number" />
      <input v-model="formato.fechaDescongelacion" placeholder="Fecha Descongelación" type="date" />
      <input v-model="formato.fechaProduccion" placeholder="Fecha Producción" type="date" />
      <input v-model="formato.realizadoPor" placeholder="Realizado por" />
      <input v-model="formato.revisadoPor" placeholder="Revisado por" />

      <h3>Registros</h3>
      <div v-for="(r, i) in formato.registros" :key="i" class="registro">
  <label>
    Número de coche:
    <input v-model="r.numeroCoche" />
  </label>
  <label>
    Código producto:
    <input v-model="r.codigoProducto" />
  </label>
  <label>
    Hora inicio descongelado:
    <input v-model="r.horaInicioDescongelado" type="time" />
  </label>
  <label>
    Temperatura:
    <input v-model="r.temperaturaProducto" type="number" />
  </label>
  <label>
    Hora inicio consumo:
    <input v-model="r.horaInicioConsumo" type="time" />
  </label>
  <label>
    Hora fin consumo:
    <input v-model="r.horaFinConsumo" type="time" />
  </label>
  <label>
    Observaciones:
    <input v-model="r.observaciones" />
  </label>
</div>

      <button type="button" @click="addRegistro">Agregar registro</button>

      <button type="submit">Guardar Formato</button>
    </form>
  </div>
</template>

<script>
import { reactive } from 'vue';
import { useFormatoStore } from '../stores/formatoStore';

export default {
  setup() {
    const store = useFormatoStore();

    const formato = reactive({
      destinoId: null,
      fechaDescongelacion: '',
      fechaProduccion: '',
      realizadoPor: '',
      revisadoPor: '',
      registros: [],
    });

    const addRegistro = () => {
      formato.registros.push({
        numeroCoche: '',
        codigoProducto: '',
        horaInicioDescongelado: '',
        temperaturaProducto: null,
        horaInicioConsumo: '',
        horaFinConsumo: '',
        observaciones: '',
      });
    };

    // 🔧 Normaliza "HH:mm" -> "HH:mm:ss"
    function normalizeTime(value) {
      if (!value) return null; 
      return value && value.length === 5 ? value + ":00" : value;
    }

    const submit = async () => {
      const payload = {
        ...formato,
        registros: formato.registros.map(r => ({
          ...r,
          horaInicioDescongelado: normalizeTime(r.horaInicioDescongelado),
          horaInicioConsumo: normalizeTime(r.horaInicioConsumo),
          horaFinConsumo: normalizeTime(r.horaFinConsumo),
        }))
      };

      await store.addFormato(payload);
      alert('Formato guardado!');
    };

    return { formato, addRegistro, submit };
  },
};
</script>
