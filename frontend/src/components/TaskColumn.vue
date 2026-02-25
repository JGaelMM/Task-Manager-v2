<template>
  <div
    :class="[
      'flex flex-col h-full rounded-2xl p-4 transition-all duration-200',
      'bg-zinc-50 dark:bg-zinc-900/50',
      isDragOver
        ? 'bg-blue-50 dark:bg-blue-950/30 ring-2 ring-blue-400/40 ring-dashed'
        : ''
    ]"
    @dragover.prevent="onDragOver"
    @dragleave.prevent="onDragLeave"
    @drop.prevent="onDrop"
  >
    <!-- Cabecera -->
    <div class="flex items-center justify-between mb-4 px-1 shrink-0">
      <div class="flex items-center gap-2">
        <!-- Ícono SVG por estado -->
        <span class="text-zinc-400 dark:text-zinc-500">
          <svg v-if="status === 'Pendiente'" xmlns="http://www.w3.org/2000/svg" width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <circle cx="12" cy="12" r="10"/><line x1="12" y1="8" x2="12" y2="12"/><line x1="12" y1="16" x2="12.01" y2="16"/>
          </svg>
          <svg v-else-if="status === 'En proceso'" xmlns="http://www.w3.org/2000/svg" width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <polyline points="13 2 13 9 18 9"/><path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V9z"/>
          </svg>
          <svg v-else-if="status === 'Revisión'" xmlns="http://www.w3.org/2000/svg" width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <circle cx="11" cy="11" r="8"/><line x1="21" y1="21" x2="16.65" y2="16.65"/>
          </svg>
          <svg v-else xmlns="http://www.w3.org/2000/svg" width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <polyline points="20 6 9 17 4 12"/>
          </svg>
        </span>

        <h2 class="text-xs font-bold text-zinc-500 dark:text-zinc-400 uppercase tracking-wider">
          {{ status }}
        </h2>
        <span :class="['px-1.5 py-0.5 rounded-full text-xs font-bold', countBadge]">
          {{ tasks.length }}
        </span>
      </div>
    </div>

    <!-- Lista de tareas -->
    <div class="flex flex-col gap-3 overflow-y-auto pb-2 flex-1 min-h-0">
      <TaskCard
        v-for="task in tasks"
        :key="task.id"
        :task="task"
        @edit="$emit('edit', $event)"
        @delete="$emit('delete', $event)"
      />

      <!-- Zona vacía -->
      <div
        v-if="tasks.length === 0"
        :class="[
          'h-20 border-2 border-dashed rounded-xl flex items-center justify-center text-xs transition-colors',
          isDragOver
            ? 'border-blue-400 text-blue-400 bg-blue-50/50 dark:bg-blue-950/20'
            : 'border-zinc-200 dark:border-zinc-700 text-zinc-400'
        ]"
      >
        Arrastra una tarea aquí
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import TaskCard from './TaskCard.vue'

const props = defineProps({
  status: { type: String, required: true },
  tasks: { type: Array, default: () => [] }
})

const emit = defineEmits(['taskDrop', 'edit', 'delete'])

const isDragOver = ref(false)
let dragCounter = 0

const countBadge = computed(() => ({
  'Pendiente':  'bg-zinc-200 dark:bg-zinc-800 text-zinc-600 dark:text-zinc-400',
  'En proceso': 'bg-blue-100 dark:bg-blue-900/40 text-blue-700 dark:text-blue-300',
  'Revisión':   'bg-amber-100 dark:bg-amber-900/40 text-amber-700 dark:text-amber-300',
  'Finalizado': 'bg-emerald-100 dark:bg-emerald-900/40 text-emerald-700 dark:text-emerald-300',
}[props.status] || 'bg-zinc-200 text-zinc-600'))

function onDragOver(e) {
  e.dataTransfer.dropEffect = 'move'
  isDragOver.value = true
}

function onDragLeave(e) {
  // Solo quitar el highlight si realmente salimos del contenedor
  if (!e.currentTarget.contains(e.relatedTarget)) {
    isDragOver.value = false
  }
}

function onDrop(e) {
  isDragOver.value = false
  const taskId = e.dataTransfer.getData('taskId')
  const fromStatus = e.dataTransfer.getData('fromStatus')

  if (taskId && fromStatus !== props.status) {
    emit('taskDrop', { taskId: parseInt(taskId) || taskId, newStatus: props.status })
  }
}
</script>