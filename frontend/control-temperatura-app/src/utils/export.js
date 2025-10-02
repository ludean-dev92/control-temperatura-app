import * as XLSX from 'xlsx';
import { saveAs } from 'file-saver';
import jsPDF from 'jspdf';
import 'jspdf-autotable';

// Exportar Excel
export function exportToExcel(data, filename = 'reporte.xlsx') {
  const worksheet = XLSX.utils.json_to_sheet(data);
  const workbook = XLSX.utils.book_new();
  XLSX.utils.book_append_sheet(workbook, worksheet, 'Reporte');
  const excelBuffer = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
  const blob = new Blob([excelBuffer], { type: 'application/octet-stream' });
  saveAs(blob, filename);
}

// Exportar PDF
export function exportToPDF(data, filename = 'reporte.pdf') {
  const doc = new jsPDF();
  const columns = Object.keys(data[0] || {}).map(key => ({ header: key, dataKey: key }));

  doc.autoTable({
    columns,
    body: data
  });

  doc.save(filename);
}
