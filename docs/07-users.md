# Usuarios

> Especificación funcional del módulo de gestión de usuarios.

---

# Objetivo

El módulo de Usuarios permite administrar las cuentas de la plataforma de forma segura y centralizada.

Su propósito es facilitar la gestión de usuarios, roles y permisos, garantizando una autenticación robusta y una experiencia de administración sencilla.

---

# Funcionalidades principales

El módulo permitirá:

- Consultar usuarios
- Buscar usuarios
- Filtrar usuarios
- Crear usuarios
- Editar usuarios
- Eliminar usuarios
- Cambiar el estado de una cuenta
- Asignar roles

---

# Listado de usuarios

La pantalla principal mostrará:

| Campo | Descripción |
|--------|-------------|
| Nombre | Nombre completo |
| Correo | Dirección de correo |
| Rol | Rol asignado |
| Estado | Activo / Inactivo |
| Fecha de alta | Registro del usuario |
| Acciones | Ver, Editar, Eliminar |

---

# Búsqueda

Será posible buscar por:

- Nombre
- Correo electrónico

Los resultados se actualizarán automáticamente.

---

# Filtros

Se podrán aplicar filtros por:

- Rol
- Estado

---

# Roles

Inicialmente existirán los siguientes roles:

- Administrador
- Gestor
- Cliente

Cada rol determinará los permisos disponibles.

---

# Crear usuario

El formulario incluirá:

- Nombre
- Apellidos
- Correo electrónico
- Contraseña
- Confirmación de contraseña
- Rol

Validaciones:

- Todos los campos obligatorios
- Correo válido
- Contraseña segura

---

# Editar usuario

Permitirá modificar:

- Nombre
- Apellidos
- Correo
- Rol
- Estado

---

# Eliminar usuario

Antes de eliminar una cuenta se solicitará confirmación.

Siempre que sea posible se realizará una eliminación lógica.

---

# Perfil del usuario

Cada usuario podrá consultar:

- Datos personales
- Rol
- Fecha de creación
- Último acceso

---

# Componentes reutilizables

Este módulo utilizará:

- UsersTable
- UserCard
- UserForm
- UserFilters
- SearchBar
- Pagination
- ConfirmDialog
- EmptyState
- LoadingState

---

# API

## Obtener usuarios

GET /api/usuarios

## Obtener usuario

GET /api/usuarios/{id}

## Crear usuario

POST /api/usuarios

## Editar usuario

PUT /api/usuarios/{id}

## Eliminar usuario

DELETE /api/usuarios/{id}

---

# Estados de la interfaz

Se contemplarán los siguientes estados:

- Cargando
- Lista vacía
- Error
- Sin resultados
- Operación completada

---

# Responsive

## Escritorio

Tabla completa.

## Tablet

Tabla adaptada.

## Móvil

Visualización mediante tarjetas.

---

# Seguridad

El acceso a este módulo estará restringido a usuarios con permisos suficientes.

Las acciones sensibles requerirán autenticación válida.

---

# Mejoras futuras

- Restablecimiento de contraseña
- Autenticación multifactor (MFA)
- Historial de accesos
- Auditoría de cambios
- Bloqueo automático de cuentas

---

# Criterios de aceptación

El módulo se considerará finalizado cuando:

- Permita gestionar usuarios y roles.
- Muestre información obtenida desde la API.
- Sea completamente responsive.
- Utilice componentes reutilizables.
- Cumpla las normas del Design System.
