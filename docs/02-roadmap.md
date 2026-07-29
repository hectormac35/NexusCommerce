# Roadmap de NexusCommerce

> Estado actual del proyecto y planificación de desarrollo.

---

# Objetivo

Este documento define el estado real del proyecto, los sprints completados y las funcionalidades previstas.

El objetivo es mantener una planificación clara durante todo el desarrollo y asegurar que todas las decisiones siguen una misma dirección.

---

# Estado general

| Área | Estado |
|------|--------|
| Backend | 🟢 Completado |
| Frontend | 🟡 En desarrollo |
| Infraestructura | 🟢 Completada |
| Observabilidad | 🟡 En desarrollo |
| Azure | ⚪ Pendiente |
| Documentación | 🟡 En desarrollo |

---

# Sprint 0 — Infraestructura

## Estado

✅ Completado

## Objetivos alcanzados

- Microservicios
- Clean Architecture
- CQRS
- MediatR
- PostgreSQL
- RabbitMQ
- JWT
- Refresh Token
- API Gateway (YARP)
- Docker
- Kubernetes
- OpenTelemetry
- Jaeger
- Health Checks

---

# Sprint 1 — Foundation UI

## Estado

🟡 En progreso

## Objetivo

Crear la base visual completa de la aplicación.

## Tareas

- [x] React
- [x] Tailwind CSS
- [x] shadcn/ui
- [x] Sidebar inicial
- [x] Topbar inicial
- [x] Dashboard inicial
- [ ] Dashboard Premium
- [ ] Componentes reutilizables
- [ ] Tema definitivo
- [ ] Responsive completo

---

# Sprint 2 — Dashboard Premium

## Estado

⬜ Pendiente

## Objetivos

- KPIs profesionales
- Gráficos
- Productos recientes
- Actividad reciente
- Platform Health
- Quick Actions

---

# Sprint 3 — Catálogo

## Estado

⬜ Pendiente

## Objetivos

- Tabla profesional
- Tarjetas
- Búsqueda
- Filtros
- Paginación
- CRUD completo
- Optimistic Updates

---

# Sprint 4 — Identity

## Estado

⬜ Pendiente

## Objetivos

- Perfil
- Avatar
- Roles
- Registro
- Refresh automático
- Protección de rutas

---

# Sprint 5 — Platform Health

## Estado

⬜ Pendiente

## Objetivos

- Estado Gateway
- Estado Identity
- Estado Catalog
- Estado RabbitMQ
- Estado PostgreSQL
- Estado Kubernetes
- Estado Jaeger

---

# Sprint 6 — Observabilidad

## Estado

⬜ Pendiente

## Objetivos

- Métricas
- Logs
- Tracing
- Dashboards

---

# Sprint 7 — Azure

## Estado

⬜ Pendiente

## Objetivos

- Azure Container Apps o AKS
- Azure Database PostgreSQL
- Azure Container Registry
- CI/CD
- URL pública

---

# Sprint 8 — Calidad

## Estado

⬜ Pendiente

## Objetivos

- Tests Frontend
- Tests E2E
- Accesibilidad
- Optimización
- Lighthouse
- SEO técnico

---

# Versión 1.0

La versión 1.0 se considerará finalizada cuando se cumplan los siguientes requisitos.

## Backend

- [x] Microservicios
- [x] Gateway
- [x] RabbitMQ
- [x] PostgreSQL
- [x] Kubernetes

## Frontend

- [ ] Dashboard Premium
- [ ] Catálogo Premium
- [ ] Gestión de usuarios
- [ ] Platform Health
- [ ] Responsive

## Cloud

- [ ] Azure
- [ ] Dominio
- [ ] HTTPS

## Documentación

- [ ] Architecture
- [ ] Design System
- [ ] Dashboard
- [ ] Deployment
- [ ] ADR

---

# Próximo objetivo

Finalizar completamente el Sprint 1 (Foundation UI) y comenzar el Dashboard Premium.
