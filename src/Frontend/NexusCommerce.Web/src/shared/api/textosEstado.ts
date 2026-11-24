// Los servicios responden con estados en inglés (Healthy, running…);
// aquí se traducen para mostrarlos en el panel.
const estados: Record<string, string> = {
  healthy: 'Operativo',
  degraded: 'Degradado',
  unhealthy: 'Caído',
  running: 'Activa',
  idle: 'Inactiva',
  flow: 'Limitada',
  stopped: 'Detenida',
  unknown: 'Desconocido',
}

export function textoEstado(estado?: string | null): string {
  if (!estado) {
    return 'Desconocido'
  }

  return estados[estado.toLowerCase()] ?? estado
}
