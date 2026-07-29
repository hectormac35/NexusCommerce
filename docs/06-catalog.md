# Catálogo

> Especificación funcional del módulo de gestión de productos.

---

# Objetivo

El módulo Catálogo permite administrar los productos de la plataforma de forma sencilla, rápida y segura.

Su diseño está orientado a ofrecer una experiencia similar a la de aplicaciones SaaS profesionales, permitiendo gestionar un gran volumen de información de manera eficiente.

---

# Funcionalidades principales

El módulo deberá permitir:

- Consultar productos
- Buscar productos
- Filtrar productos
- Ordenar resultados
- Crear productos
- Editar productos
- Eliminar productos
- Activar o desactivar productos

---

# Listado de productos

La pantalla principal mostrará una tabla con la siguiente información:

| Campo | Descripción |
|---|---|
| Nombre | Nombre del producto |
| Categoría | Categoría asignada |
| Precio | Precio actual |
| Stock | Unidades disponibles |
| Estado | Activo o inactivo |
| Fecha de creación | Fecha de alta |
| Acciones | Ver, editar y eliminar |

---

# Búsqueda

El usuario podrá buscar productos por:

- Nombre
- Categoría

La búsqueda deberá actualizar los resultados sin recargar la página.

---

# Filtros

Se incluirán filtros para:

- Categoría
- Estado
- Disponibilidad de stock

Los filtros podrán combinarse entre sí.

---

# Ordenación

La tabla permitirá ordenar por:

- Nombre
- Precio
- Stock
- Fecha de creación

---

# Paginación

El listado utilizará paginación para mejorar el rendimiento.

Opciones:

- 10 elementos
- 25 elementos
- 50 elementos
- 100 elementos

---

# Crear producto

El formulario permitirá introducir:

- Nombre
- Descripción
- Categoría
- Precio
- Stock

Validaciones:

- Nombre obligatorio
- Precio mayor que cero
- Stock igual o superior a cero

---

# Editar producto

El usuario podrá modificar todos los campos del producto.

Las modificaciones se reflejarán inmediatamente en la interfaz.

---

# Eliminar producto

Antes de eliminar un producto se mostrará un cuadro de confirmación.

La eliminación será lógica siempre que sea posible.

---

# Estado del producto

Cada producto podrá encontrarse en uno de los siguientes estados:

- Activo
- Inactivo

Los productos inactivos seguirán existiendo en la base de datos.

---

# Componentes reutilizables

Este módulo utilizará los siguientes componentes:

- ProductTable
- ProductCard
- ProductForm
- ProductFilters
- SearchBar
- Pagination
- ConfirmDialog
- EmptyState
- LoadingState

---

# API

## Obtener productos

`GET /api/catalogo/productos`

## Obtener producto

`GET /api/catalogo/productos/{id}`

## Crear producto

`POST /api/catalogo/productos`

## Editar producto

`PUT /api/catalogo/productos/{id}`

## Eliminar producto

`DELETE /api/catalogo/productos/{id}`

---

# Estados de la interfaz

El módulo deberá contemplar:

- Cargando
- Sin resultados
- Error de conexión
- Lista vacía
- Operación completada

---

# Rendimiento

El listado deberá:

- Utilizar paginación
- Evitar recargas completas
- Actualizar únicamente los datos necesarios
- Mantener una experiencia fluida

---

# Responsive

## Escritorio

Tabla completa.

## Tablet

Tabla adaptada con menos columnas visibles.

## Móvil

Visualización mediante tarjetas.

---

# Mejoras futuras

En próximas versiones se podrán añadir:

- Importación desde CSV
- Exportación a Excel
- Gestión de imágenes
- Historial de cambios
- Duplicar producto
- Acciones masivas

---

# Criterios de aceptación

El módulo Catálogo se considerará finalizado cuando:

- Permita realizar todas las operaciones CRUD.
- Muestre datos reales desde la API.
- Sea completamente responsive.
- Utilice componentes reutilizables.
- Cumpla las normas definidas en el Design System.
