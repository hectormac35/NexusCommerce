import {
  Activity,
  Info,
  MonitorCog,
  RefreshCcw,
  RotateCcw,
  Settings,
} from 'lucide-react'

import { Button } from '@/components/ui/button'
import { useSettingsStore } from '../store/settingsStore'
import type { MonitoringInterval } from '../types/settings'

const intervals: Array<{
  value: MonitoringInterval
  label: string
  description: string
}> = [
  {
    value: 5_000,
    label: '5 segundos',
    description: 'Actualización frecuente',
  },
  {
    value: 10_000,
    label: '10 segundos',
    description: 'Recomendado',
  },
  {
    value: 30_000,
    label: '30 segundos',
    description: 'Menor consumo',
  },
  {
    value: 60_000,
    label: '1 minuto',
    description: 'Actualización relajada',
  },
  {
    value: 0,
    label: 'Desactivado',
    description: 'Solo actualización manual',
  },
]

export function SettingsPage() {
  const monitoringInterval = useSettingsStore(
    (state) => state.monitoringInterval,
  )

  const refreshInBackground = useSettingsStore(
    (state) => state.refreshInBackground,
  )

  const setMonitoringInterval = useSettingsStore(
    (state) => state.setMonitoringInterval,
  )

  const setRefreshInBackground = useSettingsStore(
    (state) => state.setRefreshInBackground,
  )

  const resetSettings = useSettingsStore(
    (state) => state.resetSettings,
  )

  return (
    <div className="mx-auto w-full max-w-7xl space-y-8">
      <section>
        <div className="flex items-center gap-2 text-sm font-medium text-blue-400">
          <Settings size={17} />
          Preferencias del sistema
        </div>

        <h1 className="mt-2 text-3xl font-bold tracking-tight text-white">
          Configuración
        </h1>

        <p className="mt-2 max-w-3xl text-sm leading-6 text-slate-400">
          Personaliza el comportamiento del panel de administración y
          controla cómo NexusCommerce actualiza la información de
          observabilidad.
        </p>
      </section>

      <section className="overflow-hidden rounded-2xl border border-slate-800 bg-slate-900/60">
        <div className="border-b border-slate-800 p-6">
          <div className="flex items-start gap-4">
            <div className="rounded-xl bg-blue-500/10 p-3 text-blue-400">
              <Activity size={21} />
            </div>

            <div>
              <h2 className="font-semibold text-white">
                Monitorización
              </h2>

              <p className="mt-1 text-sm text-slate-400">
                Controla la frecuencia con la que se actualizan
                Plataforma, RabbitMQ y Jaeger.
              </p>
            </div>
          </div>
        </div>

        <div className="space-y-6 p-6">
          <div>
            <p className="text-sm font-semibold text-slate-200">
              Intervalo de actualización
            </p>

            <p className="mt-1 text-sm text-slate-500">
              Esta preferencia se guarda automáticamente en este
              navegador.
            </p>

            <div className="mt-4 grid gap-3 sm:grid-cols-2 xl:grid-cols-5">
              {intervals.map((interval) => {
                const selected =
                  monitoringInterval === interval.value

                return (
                  <button
                    key={interval.value}
                    type="button"
                    onClick={() =>
                      setMonitoringInterval(interval.value)
                    }
                    className={[
                      'rounded-xl border p-4 text-left transition',
                      selected
                        ? 'border-blue-500 bg-blue-500/10'
                        : 'border-slate-800 bg-slate-950/50 hover:border-slate-700 hover:bg-slate-900',
                    ].join(' ')}
                  >
                    <div className="flex items-center gap-2">
                      <RefreshCcw
                        size={16}
                        className={
                          selected
                            ? 'text-blue-400'
                            : 'text-slate-500'
                        }
                      />

                      <span
                        className={[
                          'text-sm font-semibold',
                          selected
                            ? 'text-blue-300'
                            : 'text-slate-200',
                        ].join(' ')}
                      >
                        {interval.label}
                      </span>
                    </div>

                    <p className="mt-2 text-xs text-slate-500">
                      {interval.description}
                    </p>
                  </button>
                )
              })}
            </div>
          </div>

          <div className="flex flex-col gap-4 rounded-xl border border-slate-800 bg-slate-950/50 p-5 sm:flex-row sm:items-center sm:justify-between">
            <div className="flex items-start gap-3">
              <MonitorCog
                size={20}
                className="mt-0.5 text-slate-400"
              />

              <div>
                <p className="text-sm font-semibold text-slate-200">
                  Actualizar en segundo plano
                </p>

                <p className="mt-1 text-sm text-slate-500">
                  Mantiene actualizadas las métricas aunque la pestaña
                  del navegador no esté activa.
                </p>
              </div>
            </div>

            <button
              type="button"
              role="switch"
              aria-checked={refreshInBackground}
              onClick={() =>
                setRefreshInBackground(!refreshInBackground)
              }
              className={[
                'relative h-7 w-12 shrink-0 rounded-full transition',
                refreshInBackground
                  ? 'bg-blue-600'
                  : 'bg-slate-700',
              ].join(' ')}
            >
              <span
                className={[
                  'absolute top-1 h-5 w-5 rounded-full bg-white transition-all',
                  refreshInBackground
                    ? 'left-6'
                    : 'left-1',
                ].join(' ')}
              />
            </button>
          </div>
        </div>
      </section>

      <section className="grid gap-6 lg:grid-cols-2">
        <div className="rounded-2xl border border-slate-800 bg-slate-900/60 p-6">
          <div className="flex items-center gap-3">
            <div className="rounded-xl bg-violet-500/10 p-3 text-violet-400">
              <Info size={20} />
            </div>

            <div>
              <h2 className="font-semibold text-white">
                Aplicación
              </h2>

              <p className="text-sm text-slate-500">
                Información del panel
              </p>
            </div>
          </div>

          <div className="mt-6 space-y-4 text-sm">
            <div className="flex justify-between gap-4 border-b border-slate-800 pb-3">
              <span className="text-slate-500">
                Aplicación
              </span>
              <span className="font-medium text-slate-200">
                NexusCommerce
              </span>
            </div>

            <div className="flex justify-between gap-4 border-b border-slate-800 pb-3">
              <span className="text-slate-500">
                Interfaz
              </span>
              <span className="font-medium text-slate-200">
                React + TypeScript
              </span>
            </div>

            <div className="flex justify-between gap-4">
              <span className="text-slate-500">
                API Gateway
              </span>
              <span
                translate="no"
                className="notranslate font-mono text-xs text-slate-300"
              >
                {import.meta.env.VITE_API_URL ??
                  'No configurado'}
              </span>
            </div>
          </div>
        </div>

        <div className="rounded-2xl border border-slate-800 bg-slate-900/60 p-6">
          <div className="flex items-center gap-3">
            <div className="rounded-xl bg-amber-500/10 p-3 text-amber-400">
              <RotateCcw size={20} />
            </div>

            <div>
              <h2 className="font-semibold text-white">
                Restaurar preferencias
              </h2>

              <p className="text-sm text-slate-500">
                Recupera la configuración inicial
              </p>
            </div>
          </div>

          <p className="mt-6 text-sm leading-6 text-slate-400">
            Restablece el intervalo de monitorización a 10 segundos
            y activa nuevamente las actualizaciones en segundo plano.
          </p>

          <Button
            type="button"
            variant="outline"
            onClick={resetSettings}
            className="mt-5"
          >
            <RotateCcw size={16} />
            Restaurar valores
          </Button>
        </div>
      </section>
    </div>
  )
}
