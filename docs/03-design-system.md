# Design System

> Guía oficial de diseño de NexusCommerce.

---

# Objetivo

El Design System define todas las reglas visuales de NexusCommerce.

Su finalidad es garantizar una experiencia de usuario consistente, escalable y profesional.

Todos los componentes del frontend deberán respetar estas normas.

---

# Filosofía

El diseño está inspirado en plataformas modernas como:

- GitHub
- Azure Portal
- Linear
- Stripe Dashboard
- Grafana

No se pretende copiar su apariencia.

El objetivo es adoptar los mismos principios de diseño:

- Simplicidad
- Consistencia
- Alta densidad de información
- Excelente experiencia de usuario
- Componentes reutilizables

---

# Paleta de colores

## Fondo principal

020617

---

## Superficie

0F172A

---

## Superficie secundaria

111827

---

## Bordes

1E293B

---

## Color primario

2563EB

---

## Éxito

22C55E

---

## Advertencia

F59E0B

---

## Error

EF4444

---

## Texto principal

F8FAFC

---

## Texto secundario

94A3B8

---

# Tipografía

## H1

36 px

Peso 700

---

## H2

30 px

Peso 700

---

## H3

24 px

Peso 600

---

## Texto

14 px

Peso 400

---

## Caption

12 px

Peso 400

---

# Espaciado

| Nombre | Valor |
|---------|-------|
| xs | 4 px |
| sm | 8 px |
| md | 16 px |
| lg | 24 px |
| xl | 32 px |
| 2xl | 48 px |

---

# Bordes

Todos los componentes principales utilizarán:

rounded-2xl

---

# Sombras

Muy suaves.

Nunca utilizar sombras excesivas.

El protagonismo debe recaer en el contenido.

---

# Animaciones

Duración estándar

200 ms

Curva

ease-in-out

Las animaciones deben utilizarse únicamente para mejorar la experiencia de usuario.

Nunca deberán distraer.

---

# Iconografía

Toda la aplicación utilizará:

Lucide React

No se mezclarán librerías de iconos.

---

# Componentes oficiales

Los únicos componentes base permitidos son:

- Button
- Card
- Badge
- Input
- Avatar
- Dropdown Menu
- Tooltip
- Separator
- Sheet
- Dialog
- Table
- Tabs
- Toast

Todos ellos estarán construidos sobre shadcn/ui.

---

# Layout

La aplicación estará compuesta por:

Sidebar

↓

Topbar

↓

Content

↓

Footer

---

# Dashboard

Todo Dashboard deberá contener:

- KPIs
- Estado de la plataforma
- Información reciente
- Acciones rápidas

---

# Responsive

La aplicación deberá funcionar correctamente en:

- Escritorio
- Portátil
- Tablet
- Móvil

---

# Accesibilidad

Todos los componentes deberán cumplir:

- Navegación mediante teclado
- Focus visible
- Contraste suficiente
- Etiquetas accesibles

---

# Principios

Cada pantalla debe responder a estas preguntas.

## ¿Es clara?

## ¿Es consistente?

## ¿Es rápida?

## ¿Es reutilizable?

## ¿Podría mantenerse durante años?

Si la respuesta es "no" a cualquiera de estas preguntas, el diseño deberá revisarse.

---

# Regla principal

Antes de crear un nuevo componente deberá comprobarse si ya existe uno reutilizable.

La duplicación visual queda prohibida.

Toda mejora realizada sobre un componente deberá beneficiar automáticamente al resto de la aplicación.
