# Platform Health

> Especificación funcional del panel de monitorización de la plataforma.

---

# Objetivo

Platform Health proporciona una visión centralizada del estado de todos los servicios que forman NexusCommerce.

Su finalidad es permitir detectar rápidamente problemas de disponibilidad, rendimiento o infraestructura.

Este módulo representa uno de los principales elementos diferenciadores del proyecto.

---

# Servicios monitorizados

La plataforma mostrará el estado de los siguientes servicios:

- Gateway
- Identity
- Catalog
- RabbitMQ
- PostgreSQL
- Kubernetes
- Jaeger

---

# Información mostrada

Cada servicio mostrará:

- Estado
- Tiempo de respuesta
- Última comprobación
- Disponibilidad
- Observaciones

---

# Estados posibles

| Estado | Descripción |
|---------|-------------|
| 🟢 Healthy | Servicio funcionando correctamente |
| 🟡 Degraded | Servicio operativo con incidencias |
| 🔴 Unhealthy | Servicio no disponible |

---

# Diseño

Cada servicio aparecerá como una tarjeta independiente.

Ejemplo:

+------------------------------------------------------+
| 🟢 Catalog API                                       |
| Estado: Healthy                                      |
| Tiempo de respuesta: 24 ms                           |
| Última comprobación: hace 8 segundos                 |
+------------------------------------------------------+

---

# Gateway

Información mostrada:

- Estado
- Tiempo de respuesta

Endpoint:

GET /health

---

# Identity

Información mostrada:

- Estado
- Tiempo de respuesta
- Conectividad con PostgreSQL

---

# Catalog

Información mostrada:

- Estado
- Tiempo de respuesta
- Estado de la base de datos

---

# RabbitMQ

Información mostrada:

- Conectividad
- Cola de mensajes
- Estado general

---

# PostgreSQL

Información mostrada:

- Estado
- Tiempo de respuesta
- Disponibilidad

---

# Kubernetes

Información mostrada:

- Pods activos
- Pods con errores
- Servicios desplegados

---

# Jaeger

Información mostrada:

- Disponibilidad
- Estado del servicio

---

# Actualización automática

El Dashboard actualizará la información automáticamente sin necesidad de recargar la página.

---

# Componentes reutilizables

Se utilizarán los siguientes componentes:

- HealthCard
- HealthGrid
- StatusBadge
- ResponseTime
- LastCheck
- LoadingState
- ErrorState

---

# API

GET /health

GET /health/catalog

GET /health/identity

GET /health/rabbitmq

GET /health/postgresql

GET /health/kubernetes

---

# Estados de la interfaz

- Cargando
- Información disponible
- Error de conexión
- Servicio no disponible

---

# Responsive

## Escritorio

Cuadrícula de varias columnas.

## Tablet

Dos columnas.

## Móvil

Una única columna.

---

# Mejoras futuras

- Consumo de CPU
- Uso de memoria
- Uso de disco
- Número de peticiones
- Trazas distribuidas
- Eventos en tiempo real mediante SignalR

---

# Criterios de aceptación

Platform Health se considerará finalizado cuando:

- Muestre el estado de todos los servicios.
- Obtenga datos reales desde la API.
- Actualice automáticamente la información.
- Sea completamente responsive.
- Cumpla el Design System.
