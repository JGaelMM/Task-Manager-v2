<template>
  <div
    :class="[
      'relative bg-white dark:bg-zinc-900 rounded-xl border transition-all duration-200 select-none',
      'border-zinc-200 dark:border-zinc-800',
      isDragging ? 'opacity-40 scale-95 shadow-2xl rotate-1' : 'opacity-100',
      isExpanded ? 'shadow-lg ring-1 ring-blue-500/20' : 'shadow-sm hover:shadow-md',
      'cursor-grab active:cursor-grabbing'
    ]"
    draggable="true"
    @dragstart="onDragStart"
    @dragend="onDragEnd"
    @click="isExpanded = !isExpanded"
  >
    <!-- Barra lateral de prioridad -->
    <div :class="['absolute left-0 top-0 bottom-0 w-1 rounded-l-xl transition-colors duration-200', priorityBar]" />

    <div class="p-4 pl-5">
      <!-- Cabecera -->
      <div class="flex justify-between items-start mb-2">
        <span :class="['px-2 py-0.5 rounded-md text-[10px] font-bold uppercase tracking-wide', priorityBadge]">
          {{ task.priority }}
        </span>

        <transition name="fade">
          <div v-if="isExpanded" class="flex gap-1" @click.stop>
            <button
              @click="$emit('edit', task)"
              class="p-1.5 rounded-full text-zinc-400 hover:text-blue-600 hover:bg-blue-50 dark:hover:bg-blue-900/20 transition-colors"
              title="Editar"
            >
              <!-- Edit SVG -->
              <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"/>
                <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"/>
              </svg>
            </button>
            <button
              @click="$emit('delete', task.id)"
              class="p-1.5 rounded-full text-zinc-400 hover:text-red-600 hover:bg-red-50 dark:hover:bg-red-900/20 transition-colors"
              title="Eliminar"
            >
              <!-- Trash SVG -->
              <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <polyline points="3 6 5 6 21 6"/>
                <path d="M19 6l-1 14a2 2 0 0 1-2 2H8a2 2 0 0 1-2-2L5 6"/>
                <path d="M10 11v6M14 11v6"/>
                <path d="M9 6V4a1 1 0 0 1 1-1h4a1 1 0 0 1 1 1v2"/>
              </svg>
            </button>
          </div>
        </transition>
      </div>

      <!-- Título -->
      <h3 class="text-sm font-semibold text-zinc-900 dark:text-zinc-100 leading-tight">
        {{ task.title }}
      </h3>

      <!-- Colapsado: fecha -->
      <div v-if="!isExpanded" class="flex items-center gap-1.5 text-xs text-zinc-400 mt-2">
        <!-- Calendar SVG -->
        <svg xmlns="http://www.w3.org/2000/svg" width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <rect x="3" y="4" width="18" height="18" rx="2" ry="2"/>
          <line x1="16" y1="2" x2="16" y2="6"/><line x1="8" y1="2" x2="8" y2="6"/>
          <line x1="3" y1="10" x2="21" y2="10"/>
        </svg>
        <span>{{ formatDate(task.createdAt) }}</span>
      </div>

      <!-- Expandido -->
      <transition name="expand">
        <div v-if="isExpanded" class="mt-3">
          <p class="text-sm text-zinc-500 dark:text-zinc-400 mb-3 leading-relaxed">
            {{ task.description || 'Sin descripción.' }}
          </p>
          <div class="flex items-center justify-between pt-3 border-t border-zinc-100 dark:border-zinc-800 flex-wrap gap-2">
            <span class="text-xs text-blue-600 dark:text-blue-400 bg-blue-50 dark:bg-blue-900/20 px-2 py-1 rounded-md flex items-center gap-1">
              <!-- Clock SVG -->
              <svg xmlns="http://www.w3.org/2000/svg" width="11" height="11" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <circle cx="12" cy="12" r="10"/>
                <polyline points="12 6 12 12 16 14"/>
              </svg>
              {{ task.finishedAt ? `Finalizó: ${formatDate(task.finishedAt)}` : 'En curso' }}
            </span>
            <span class="text-xs text-zinc-400">{{ formatDate(task.createdAt) }}</span>
          </div>
        </div>
      </transition>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'

const props = defineProps({
  task: { type: Object, required: true }
})

defineEmits(['edit', 'delete'])

const isExpanded = ref(false)
const isDragging = ref(false)

const priorityBadge = computed(() => ({
  'Alta':  'bg-red-100 text-red-700 dark:bg-red-900/40 dark:text-red-300',
  'Media': 'bg-amber-100 text-amber-700 dark:bg-amber-900/40 dark:text-amber-300',
  'Baja':  'bg-emerald-100 text-emerald-700 dark:bg-emerald-900/40 dark:text-emerald-300',
}[props.task.priority]))

const priorityBar = computed(() => ({
  'Alta':  'bg-red-500',
  'Media': 'bg-amber-400',
  'Baja':  'bg-emerald-500',
}[props.task.priority]))

function formatDate(dateStr) {
  if (!dateStr) return ''
  return new Date(dateStr).toLocaleDateString('es-MX', { day: '2-digit', month: 'short' })
}

function onDragStart(e) {
  isDragging.value = true
  isExpanded.value = false
  e.dataTransfer.effectAllowed = 'move'
  e.dataTransfer.setData('taskId', String(props.task.id))
  e.dataTransfer.setData('fromStatus', props.task.status)
}

function onDragEnd() {
  isDragging.value = false
}
</script>

<style scoped>
.fade-enter-active, .fade-leave-active { transition: opacity 0.15s; }
.fade-enter-from, .fade-leave-to { opacity: 0; }

.expand-enter-active, .expand-leave-active {
  transition: all 0.2s ease;
  overflow: hidden;
}
.expand-enter-from, .expand-leave-to { opacity: 0; max-height: 0; }
.expand-enter-to, .expand-leave-from { opacity: 1; max-height: 300px; }
</style>