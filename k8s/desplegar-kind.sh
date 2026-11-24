#!/usr/bin/env bash
# Despliega NexusCommerce en un clúster local de Kind.
# Requisitos: docker, kind y kubectl.
set -euo pipefail
cd "$(dirname "$0")/.."

CLUSTER=nexuscommerce

if ! kind get clusters | grep -qx "$CLUSTER"; then
  echo "▶ Creando el clúster $CLUSTER"
  kind create cluster --config k8s/kind-config.yaml
fi

declare -A IMAGENES=(
  [gateway]=src/Gateway/NexusCommerce.Gateway/Dockerfile
  [identity-api]=src/Services/Identity/Identity.Api/Dockerfile
  [catalog-api]=src/Services/Catalog/Catalog.Api/Dockerfile
  [orders-api]=src/Services/Orders/Orders.Api/Dockerfile
  [notification-worker]=src/Services/Notification/Notification.Worker/Dockerfile
)

for nombre in "${!IMAGENES[@]}"; do
  echo "▶ Construyendo nexuscommerce-$nombre:k8s"
  docker build -q -f "${IMAGENES[$nombre]}" -t "nexuscommerce-$nombre:k8s" .
  kind load docker-image "nexuscommerce-$nombre:k8s" --name "$CLUSTER"
done

echo "▶ Aplicando los manifiestos"
kubectl apply -k k8s/base

# Reinicia los despliegues para que usen las imágenes recién cargadas.
kubectl -n nexuscommerce rollout restart deployment
kubectl -n nexuscommerce wait --for=condition=available deployment --all --timeout=300s

echo "✔ Listo. Gateway: http://localhost:5080 · Jaeger: http://localhost:16686"
