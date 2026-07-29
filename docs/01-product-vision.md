# NexusCommerce

> Plataforma distribuida de comercio electrónico desarrollada con ASP.NET Core, React y Kubernetes.

---

# Visión del producto

## ¿Qué es NexusCommerce?

NexusCommerce es una plataforma distribuida desarrollada con el objetivo de demostrar cómo se diseña, construye, despliega y monitoriza una aplicación moderna utilizando tecnologías del ecosistema .NET y herramientas cloud actuales.

No pretende ser únicamente un e-commerce.

Su verdadero objetivo es servir como un proyecto de referencia que reúna en una única solución conocimientos de desarrollo backend, frontend, arquitectura de software, cloud, DevOps y observabilidad.

Cada módulo del proyecto ha sido diseñado para representar una capacidad técnica diferente, integrándose todos ellos en una única plataforma coherente.

---

# Misión

Construir una aplicación que represente la forma en la que una empresa desarrollaría un producto moderno basado en microservicios.

Cada decisión técnica debe perseguir uno o varios de los siguientes objetivos:

- Escalabilidad
- Mantenibilidad
- Reutilización
- Observabilidad
- Seguridad
- Calidad del código

El proyecto prioriza la arquitectura y la calidad de ingeniería por encima de la cantidad de funcionalidades.

---

# Objetivos del proyecto

NexusCommerce ha sido diseñado para demostrar experiencia práctica en:

- ASP.NET Core
- Clean Architecture
- CQRS
- MediatR
- PostgreSQL
- RabbitMQ
- JWT
- Refresh Tokens
- API Gateway (YARP)
- React
- TypeScript
- TanStack Query
- Zustand
- Tailwind CSS
- shadcn/ui
- Docker
- Kubernetes
- OpenTelemetry
- Jaeger
- Azure

Todas las tecnologías utilizadas tienen un propósito claro dentro de la arquitectura.

No existen tecnologías añadidas únicamente por motivos estéticos.

---

# Público objetivo

NexusCommerce no está orientado a usuarios finales.

Ha sido diseñado para ser evaluado por:

- Responsables técnicos
- Tech Leads
- Arquitectos de Software
- Recruiters técnicos
- Empresas tecnológicas

El objetivo principal es demostrar la capacidad para desarrollar una aplicación empresarial moderna.

---

# Filosofía del proyecto

El proyecto sigue una regla muy sencilla:

> Cada módulo debe demostrar un concepto de ingeniería diferente.

Por ejemplo:

| Módulo | Concepto que demuestra |
|---------|------------------------|
| Catálogo | Gestión de dominio y CRUD distribuido |
| Identity | Autenticación y autorización |
| Gateway | Reverse Proxy |
| RabbitMQ | Arquitectura orientada a eventos |
| Kubernetes | Orquestación de contenedores |
| Jaeger | Trazabilidad distribuida |
| Dashboard | Visualización de información empresarial |
| Platform Health | Monitorización de la plataforma |

---

# Principios de ingeniería

Durante el desarrollo se seguirán los siguientes principios.

## Arquitectura antes que implementación

Antes de desarrollar una funcionalidad debe existir un diseño claro.

---

## Componentes reutilizables

Toda la interfaz debe construirse mediante componentes reutilizables.

La duplicación de código debe evitarse siempre que sea posible.

---

## Preparado para producción

Cada módulo debe desarrollarse como si fuese a desplegarse en un entorno productivo.

---

## Simplicidad

Siempre se priorizará un código claro y mantenible frente a soluciones excesivamente complejas.

---

## Observabilidad

Todo aquello que sea importante deberá poder medirse.

- Estado
- Salud
- Rendimiento
- Logs
- Métricas
- Trazas

---

## Documentación

La documentación forma parte del producto.

Cada decisión importante deberá quedar registrada.

---

# Módulos actuales

La plataforma está organizada en los siguientes módulos:

- Dashboard
- Catálogo
- Identity
- Usuarios
- Configuración
- Platform Health

---

# Módulos previstos

Durante la evolución del proyecto se incorporarán nuevos módulos:

- Pedidos
- Inventario
- Notificaciones
- Analítica
- Auditoría
- Pagos

---

# Experiencia de usuario

La experiencia de usuario toma como referencia plataformas modernas como:

- GitHub
- Azure Portal
- Grafana
- Linear
- Stripe Dashboard

El objetivo no es copiar su diseño, sino aplicar los mismos principios:

- Navegación sencilla
- Alta densidad de información
- Interfaz limpia
- Componentes consistentes
- Diseño responsive
- Excelente experiencia de uso

---

# Objetivo final

La versión final de NexusCommerce deberá poder desplegarse completamente en Azure y ser accesible mediante una URL pública.

Una empresa deberá poder:

- Acceder a la aplicación.
- Iniciar sesión con un usuario de demostración.
- Explorar el Dashboard.
- Consultar el catálogo.
- Revisar el estado de la plataforma.

Sin necesidad de instalar ningún software.

---

# Criterios de éxito

El proyecto se considerará finalizado cuando cumpla los siguientes objetivos.

## Ingeniería

- Arquitectura limpia.
- Frontend modular.
- Backend distribuido.
- Automatización del despliegue.
- Documentación completa.

---

## Producto

- Interfaz profesional.
- Experiencia de usuario consistente.
- Diseño responsive.

---

## Infraestructura

- Docker.
- Kubernetes.
- Azure.
- Monitorización.
- Observabilidad.

---

## Portfolio

Un responsable técnico deberá ser capaz de comprender el proyecto dedicando únicamente unos minutos a revisar:

README

↓

Arquitectura

↓

Dashboard

↓

Código

Sin necesidad de explicaciones adicionales.
