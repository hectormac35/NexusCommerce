export type RabbitMqQueue = {
  name: string
  state: string
  messages: number
  messagesReady: number
  messagesUnacknowledged: number
  consumers: number
}

export type RabbitMqOverview = {
  status: string
  version: string
  erlangVersion: string
  clusterName: string

  published: number
  delivered: number
  acknowledged: number
  unroutable: number

  connections: number
  channels: number
  consumers: number
  queues: number

  checkedAt: string

  queueDetails: RabbitMqQueue[]
}
