<template>
  <div class="p-6 max-w-4xl mx-auto">
    <h2 class="text-2xl font-bold mb-4">Reporte Formato</h2>

    <div class="mb-4">
      <label class="block font-medium mb-1">Formato ID</label>
      <input v-model.number="formatoId" type="number" placeholder="Ingrese Formato ID" class="border p-2 rounded w-32" />
      <button @click="cargarReporte" class="bg-blue-500 hover:bg-blue-600 text-white font-bold py-2 px-4 rounded ml-2">
        Cargar
      </button>
    </div>

    <div v-if="reporte.length">
      <table class="table-auto border-collapse border border-gray-300 w-full">
        <thead>
          <tr class="bg-gray-200">
            <th class="border px-2 py-1">Coche</th>
            <th class="border px-2 py-1">Producto</th>
            <th class="border px-2 py-1">Temperatura</th>
            <th class="border px-2 py-1">Inicio Descongelado</th>
            <th class="border px-2 py-1">Inicio Consumo</th>
            <th class="border px-2 py-1">Fin Consumo</th>
            <th class="border px-2 py-1">Observaciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="r in reporte" :key="r.numeroCoche">
            <td class="border px-2 py-1">{{ r.numeroCoche }}</td>
            <td class="border px-2 py-1">{{ r.codigoProducto }}</td>
            <td class="border px-2 py-1">{{ r.temperaturaProducto }}</td>
            <td class="border px-2 py-1">{{ formatTime(r.horaInicioDescongelado) }}</td>
            <td class="border px-2 py-1">{{ formatTime(r.horaInicioConsumo) }}</td>
            <td class="border px-2 py-1">{{ formatTime(r.horaFinConsumo) }}</td>
            <td class="border px-2 py-1">{{ r.observaciones }}</td>
          </tr>
        </tbody>
      </table>

      <div class="mt-4 space-x-2">
        <button @click="exportExcel" class="bg-green-500 hover:bg-green-600 text-white py-2 px-4 rounded">Exportar Excel</button>
        <button @click="exportPDF" class="bg-red-500 hover:bg-red-600 text-white py-2 px-4 rounded">Exportar PDF</button>
      </div>
    </div>

    <div v-else class="mt-4 text-gray-600">No hay datos cargados.</div>
  </div>
</template>

<script>
import { ref } from 'vue';
import axios from 'axios';
import * as XLSX from 'xlsx';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';

export default {
  data() {
    return {
      formatoId: 1,
      reporte: [],
    };
  },
  methods: {
    async cargarReporte() {
      try {
        const res = await axios.get(`http://localhost:5169/api/reporte/${this.formatoId}`);
        this.reporte = res.data;
      } catch (err) {
        console.error(err);
        alert('Error al cargar el reporte');
      }
    },
    formatTime(time) {
      if (!time) return '';
      // Si viene como string tipo "12:30:00", cortamos minutos y horas
      if (typeof time === 'string') {
        const t = time.split(':');
        return `${t[0]}:${t[1]}`;
      }
      // Si viene como TimeSpan desde backend, convertir a HH:mm
      if (time && typeof time === 'object' && 'hours' in time && 'minutes' in time) {
        return `${time.hours}:${time.minutes}`;
      }
      return '';
    },
    exportExcel() {
      const ws = XLSX.utils.json_to_sheet(this.reporte);
      const wb = XLSX.utils.book_new();
      XLSX.utils.book_append_sheet(wb, ws, 'Reporte');
      XLSX.writeFile(wb, `Reporte_Formato_${this.formatoId}.xlsx`);
    },
    exportPDF() {
      const doc = new jsPDF();
      const columns = [
        "Coche","Producto","Temperatura","Inicio Descongelado","Inicio Consumo","Fin Consumo","Observaciones"
      ];
      const rows = this.reporte.map(r => [
        r.numeroCoche, r.codigoProducto, r.temperaturaProducto,
        this.formatTime(r.horaInicioDescongelado),
        this.formatTime(r.horaInicioConsumo),
        this.formatTime(r.horaFinConsumo),
        r.observaciones
      ]);
      
      // Usamos la función importada directamente
      autoTable(doc, { head: [columns], body: rows });

      doc.save(`Reporte_Formato_${this.formatoId}.pdf`);
    }
  }
};
</script>
