<template>
  <div class="form-container">
    <h2>Nuevo Formato</h2>
    <form @submit.prevent="submit">

      <!-- Encabezado -->
      <div class="form-grid">
        <div>
          <label>Destino ID</label>
          <input v-model.number="formato.destinoId" type="number" placeholder="Destino ID" />
        </div>
        <div>
          <label>Fecha Descongelación</label>
          <input v-model="formato.fechaDescongelacion" type="date" />
        </div>
        <div>
          <label>Fecha Producción</label>
          <input v-model="formato.fechaProduccion" type="date" />
        </div>
        <div>
          <label>Realizado por</label>
          <input v-model="formato.realizadoPor" type="text" placeholder="Nombre" />
        </div>
        <div>
          <label>Revisado por</label>
          <input v-model="formato.revisadoPor" type="text" placeholder="Nombre" />
        </div>
      </div>

      <!-- Registros -->
      <h3>Registros</h3>
      <div v-for="(r, i) in formato.registros" :key="i" class="registro">
        <div class="form-grid">
          <div>
            <label>Número de coche</label>
            <input v-model="r.numeroCoche" type="text" />
          </div>
          <div>
            <label>Código producto</label>
            <input v-model="r.codigoProducto" type="text" />
          </div>
          <div>
            <label>Temperatura</label>
            <input v-model.number="r.temperaturaProducto" type="number" />
          </div>

          <!-- Hora inicio descongelado -->
          <div>
            <label>Hora inicio descongelado</label>

            <div v-if="timeSupported">
              <input v-model="r.horaInicioDescongelado" type="time" />
            </div>

            <div v-else class="time-fallback">
              <select v-model="r.horaInicioDescongeladoHour">
                <option value="">hh</option>
                <option v-for="h in hours" :key="h" :value="h">{{ h }}</option>
              </select>
              :
              <select v-model="r.horaInicioDescongeladoMinute">
                <option value="">mm</option>
                <option v-for="m in minutes" :key="m" :value="m">{{ m }}</option>
              </select>
            </div>
          </div>

          <!-- Hora inicio consumo -->
          <div>
            <label>Hora inicio consumo</label>

            <div v-if="timeSupported">
              <input v-model="r.horaInicioConsumo" type="time" />
            </div>

            <div v-else class="time-fallback">
              <select v-model="r.horaInicioConsumoHour">
                <option value="">hh</option>
                <option v-for="h in hours" :key="h" :value="h">{{ h }}</option>
              </select>
              :
              <select v-model="r.horaInicioConsumoMinute">
                <option value="">mm</option>
                <option v-for="m in minutes" :key="m" :value="m">{{ m }}</option>
              </select>
            </div>
          </div>

          <!-- Hora fin consumo -->
          <div>
            <label>Hora fin consumo</label>

            <div v-if="timeSupported">
              <input v-model="r.horaFinConsumo" type="time" />
            </div>

            <div v-else class="time-fallback">
              <select v-model="r.horaFinConsumoHour">
                <option value="">hh</option>
                <option v-for="h in hours" :key="h" :value="h">{{ h }}</option>
              </select>
              :
              <select v-model="r.horaFinConsumoMinute">
                <option value="">mm</option>
                <option v-for="m in minutes" :key="m" :value="m">{{ m }}</option>
              </select>
            </div>
          </div>

          <div class="full-width">
            <label>Observaciones</label>
            <input v-model="r.observaciones" type="text" />
          </div>
        </div>
      </div>

      <!-- Botones -->
      <div class="form-buttons">
        <button type="button" @click="addRegistro" class="btn btn-green">Agregar registro</button>
        <button type="submit" class="btn btn-blue">Guardar Formato</button>
      </div>

    </form>
  </div>
</template>

<script>
import { reactive, ref } from 'vue';
import { useFormatoStore } from '../stores/formatoStore';
import { useToast } from 'vue-toastification';

export default {
  setup() {
    const store = useFormatoStore();
    const toast = useToast();

    // detectar soporte nativo para <input type="time">
    function supportsInputType(type) {
      const i = document.createElement('input');
      i.setAttribute('type', type);
      return i.type === type;
    }
    const timeSupported = supportsInputType('time');

    // arrays para fallback (00..23, 00..59)
    const hours = Array.from({ length: 24 }, (_, i) => String(i).padStart(2, '0'));
    const minutes = Array.from({ length: 60 }, (_, i) => String(i).padStart(2, '0'));

    const formato = reactive({
      destinoId: null,
      fechaDescongelacion: '',
      fechaProduccion: '',
      realizadoPor: '',
      revisadoPor: '',
      registros: [],
    });

    const addRegistro = () => {
      // incluir campos de fallback para hora (si se usan)
      formato.registros.push({
        numeroCoche: '',
        codigoProducto: '',
        horaInicioDescongelado: '', // si timeSupported -> valor "HH:MM"
        horaInicioDescongeladoHour: '',
        horaInicioDescongeladoMinute: '',
        temperaturaProducto: null,
        horaInicioConsumo: '',
        horaInicioConsumoHour: '',
        horaInicioConsumoMinute: '',
        horaFinConsumo: '',
        horaFinConsumoHour: '',
        horaFinConsumoMinute: '',
        observaciones: '',
      });
    };

    // helper para componer hora desde fallback o usar el input nativo
    const composeTime = (r, baseName) => {
      // si navegador soporta input type=time, usamos el campo baseName directamente
      if (timeSupported) {
        return r[baseName] ? (r[baseName].length === 5 ? r[baseName] : r[baseName] + ':00') : null;
      }
      // fallback: hour+minute
      const h = r[`${baseName}Hour`];
      const m = r[`${baseName}Minute`];
      if (!h || !m) return null;
      return `${String(h).padStart(2,'0')}:${String(m).padStart(2,'0')}`;
    };

    // Fix rápido para evitar shift de día por timezone: enviar fecha con hora mediodía
    const fixDateMidday = (d) => {
      if (!d) return null;
      // d viene como "YYYY-MM-DD" del input type=date
      return `${d}T12:00:00`;
    };

    const normalizeTimeForBackend = (time) => {
      if (!time) return null;
      // backend espera HH:mm:ss (TimeSpan/TimeOnly). Normalizamos a HH:mm:ss
      return time.length === 5 ? time + ':00' : time;
    };

    const submit = async () => {
      try {
        // Validación mínima
        if (!formato.destinoId || !formato.fechaDescongelacion || !formato.fechaProduccion) {
          toast.error('Completa los campos obligatorios del encabezado.');
          return;
        }

        const payload = {
          ...formato,
          fechaDescongelacion: fixDateMidday(formato.fechaDescongelacion),
          fechaProduccion: fixDateMidday(formato.fechaProduccion),
          registros: formato.registros.map(r => {
            const h1 = composeTime(r, 'horaInicioDescongelado');
            const h2 = composeTime(r, 'horaInicioConsumo');
            const h3 = composeTime(r, 'horaFinConsumo');

            return {
              numeroCoche: r.numeroCoche,
              codigoProducto: r.codigoProducto,
              horaInicioDescongelado: normalizeTimeForBackend(h1),
              temperaturaProducto: r.temperaturaProducto,
              horaInicioConsumo: normalizeTimeForBackend(h2),
              horaFinConsumo: normalizeTimeForBackend(h3),
              observaciones: r.observaciones
            };
          })
        };

        await store.addFormato(payload);
        toast.success('Formato guardado correctamente!');
      } catch (err) {
        console.error(err);
        toast.error('Error al guardar el formato.');
      }
    };

    // Exponer datos/funciones a template
    return {
      formato,
      addRegistro,
      submit,
      timeSupported,
      hours,
      minutes
    };
  }
};
</script>

<style scoped>
.form-container {
  max-width: 900px;
  margin: 1.5rem auto;
  padding: 1.25rem;
  background: #fff;
  border-radius: 8px;
  box-shadow: 0 6px 18px rgba(0,0,0,0.08);
  font-family: Arial, sans-serif;
}

h2, h3 {
  color: #222;
  margin-bottom: 0.75rem;
}

.form-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
  gap: 0.8rem;
  margin-bottom: 0.9rem;
}

label {
  display: block;
  font-weight: 600;
  margin-bottom: 0.25rem;
  font-size: 0.95rem;
}

input, select {
  width: 100%;
  padding: 0.45rem 0.6rem;
  border: 1px solid #d0d7de;
  border-radius: 6px;
  font-size: 0.95rem;
  box-sizing: border-box;
  background: #fff;
}

/* time fallback selects inline */
.time-fallback select {
  display: inline-block;
  width: 48%;
  margin-right: 2%;
}

/* registro card */
.registro {
  border: 1px solid #e3e7ea;
  padding: 0.9rem;
  margin-bottom: 0.9rem;
  border-radius: 8px;
  background-color: #fbfcfd;
}

/* Observaciones full width */
.full-width {
  grid-column: 1 / -1;
}

/* botones */
.form-buttons {
  display: flex;
  gap: 0.75rem;
  margin-top: 0.5rem;
  flex-wrap: wrap;
}

.btn {
  padding: 0.55rem 0.9rem;
  border-radius: 6px;
  border: none;
  font-weight: 600;
  cursor: pointer;
}

.btn-green {
  background: #16a34a;
  color: #fff;
}

.btn-blue {
  background: #0ea5e9;
  color: #fff;
}

.btn:hover { opacity: 0.95; }

/* responsive tweaks */
@media (max-width: 520px) {
  .time-fallback select { width: 46%; }
}
</style>
