# Despliegue

> Guía de despliegue y operación de NexusCommerce.

---

# Objetivo

Este documento describe cómo se construye, despliega y ejecuta NexusCommerce en un entorno moderno basado en contenedores.

La plataforma está diseñada para poder ejecutarse tanto en un entorno local de desarrollo como en un entorno cloud.

---

# Arquitectura de despliegue

La plataforma estará formada por los siguientes servicios:

- Gateway
- Identity API
- Catalog API
- Notification Worker
- RabbitMQ
- PostgreSQL (Identity)
- PostgreSQL (Catalog)
- Jaeger

Todos ellos se ejecutarán como contenedores independientes.

---

# Entorno de desarrollo

Tecnologías utilizadas:

- Docker
- Docker Compose
- Kubernetes (Kind)

El entorno de desarrollo deberá poder iniciarse mediante un único comando.

---

# Contenedores

Cada microservicio dispondrá de su propio Dockerfile.

La construcción de imágenes será independiente.

Esto permitirá desplegar únicamente los servicios modificados.

---

# Docker Compose

Docker Compose permitirá ejecutar localmente toda la plataforma.

Servicios incluidos:

- Gateway
- Identity
- Catalog
- Notification Worker
- RabbitMQ
- PostgreSQL
- Jaeger

---

# Kubernetes

La plataforma utilizará Kubernetes como sistema de orquestación.

Cada servicio contará con:

- Deployment
- Service
- Health Checks
- Variables de entorno

---

# Health Checks

Todos los servicios expondrán un endpoint:

GET /health

Estos endpoints serán utilizados tanto por Kubernetes como por el módulo Platform Health.

---

# Variables de entorno

La configuración se realizará mediante variables de entorno.

Ejemplos:

- ConnectionStrings
- JWT Secret
- RabbitMQ Host
- RabbitMQ User
- RabbitMQ Password
- ASPNETCORE_ENVIRONMENT

No deberán almacenarse secretos en el código fuente.

---

# Integración continua

El proyecto utilizará GitHub Actions para automatizar:

- Compilación
- Ejecución de pruebas
- Construcción de imágenes Docker
- Validación del código

Cada cambio en la rama principal deberá ejecutar automáticamente el flujo de integración continua.

---

# Despliegue continuo

En futuras versiones se automatizará el despliegue hacia Azure.

El flujo previsto será:

GitHub

↓

GitHub Actions

↓

Azure Container Registry

↓

Kubernetes

↓

Producción

---

# Azure

El objetivo final es desplegar NexusCommerce en Microsoft Azure.

Servicios previstos:

- Azure Container Registry
- Azure Kubernetes Service (AKS) o Azure Container Apps
- Azure Database for PostgreSQL
- Azure Monitor

---

# Observabilidad

La plataforma incorporará:

- OpenTelemetry
- Jaeger
- Health Checks

Estas herramientas permitirán monitorizar el estado de todos los servicios.

---

# Seguridad

Se seguirán las siguientes prácticas:

- Uso de HTTPS
- Gestión segura de secretos
- Variables de entorno
- JWT para autenticación
- Control de acceso basado en roles

---

# Versionado

Se utilizará Git siguiendo una estrategia basada en ramas.

Ramas principales:

- main
- develop

Las nuevas funcionalidades se desarrollarán mediante ramas independientes.

---

# Objetivo final

La versión final de NexusCommerce deberá poder desplegarse completamente desde un repositorio Git mediante un proceso automatizado.

El despliegue no deberá requerir pasos manuales.

---

# Criterios de aceptación

El sistema de despliegue se considerará finalizado cuando:

- Toda la plataforma pueda ejecutarse mediante Docker.
- Todos los servicios puedan desplegarse en Kubernetes.
- Exista una canalización de integración continua.
- El despliegue en Azure esté automatizado.
- La plataforma sea accesible mediante una URL pública segura.
