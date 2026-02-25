<template>
  <Teleport to="body">
    <Transition name="modal">
      <div
        v-if="isOpen"
        class="fixed inset-0 z-50 flex items-center justify-center p-4"
      >
        <!-- Backdrop -->
        <div class="absolute inset-0 bg-black/50 backdrop-blur-sm" @click="$emit('close')" />

        <!-- Modal -->
        <div class="relative w-full max-w-lg bg-white dark:bg-zinc-900 rounded-2xl shadow-2xl overflow-hidden max-h-[90vh] flex flex-col">
          <!-- Header -->
          <div class="flex items-center justify-between px-6 py-4 border-b border-zinc-100 dark:border-zinc-800 shrink-0">
            <div class="flex items-center gap-2">
              <!-- Plus / Edit SVG -->
              <svg v-if="mode === 'create'" xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="text-blue-600">
                <circle cx="12" cy="12" r="10"/><line x1="12" y1="8" x2="12" y2="16"/><line x1="8" y1="12" x2="16" y2="12"/>
              </svg>
              <svg v-else xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="text-blue-600">
                <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"/>
                <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"/>
              </svg>
              <h2 class="text-base font-bold text-zinc-900 dark:text-zinc-100">
                {{ mode === 'create' ? 'Nueva Tarea' : 'Editar Tarea' }}
              </h2>
            </div>
            <button
              @click="$emit('close')"
              class="p-2 rounded-full hover:bg-zinc-100 dark:hover:bg-zinc-800 text-zinc-400 hover:text-zinc-600 transition-colors"
            >
              <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <line x1="18" y1="6" x2="6" y2="18"/><line x1="6" y1="6" x2="18" y2="18"/>
              </svg>
            </button>
          </div>

          <!-- Form -->
          <form @submit.prevent="handleSubmit" class="p-6 flex flex-col gap-4 overflow-y-auto">
            <!-- Título -->
            <div class="flex flex-col gap-1.5">
              <label class="text-sm font-semibold text-zinc-700 dark:text-zinc-300">Título *</label>
              <input
                v-model="form.title"
                type="text"
                required
                placeholder="Ej: Revisar documentación"
                class="px-3 py-2.5 rounded-lg border border-zinc-300 dark:border-zinc-700 bg-white dark:bg-zinc-800 text-zinc-900 dark:text-zinc-100 focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-500 transition-all text-sm w-full"
              />
            </div>

            <!-- Descripción -->
            <div class="flex flex-col gap-1.5">
              <label class="text-sm font-semibold text-zinc-700 dark:text-zinc-300">Descripción</label>
              <textarea
                v-model="form.description"
                rows="3"
                placeholder="Detalles de la tarea..."
                class="px-3 py-2.5 rounded-lg border border-zinc-300 dark:border-zinc-700 bg-white dark:bg-zinc-800 text-zinc-900 dark:text-zinc-100 focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-500 resize-none transition-all text-sm w-full"
              />
            </div>

            <!-- Prioridad + Estado -->
            <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
              <div class="flex flex-col gap-1.5">
                <label class="text-sm font-semibold text-zinc-700 dark:text-zinc-300">Prioridad</label>
                <select
                  v-model="form.priority"
                  class="px-3 py-2.5 rounded-lg border border-zinc-300 dark:border-zinc-700 bg-white dark:bg-zinc-800 text-zinc-900 dark:text-zinc-100 focus:outline-none focus:ring-2 focus:ring-blue-500/20 transition-all text-sm w-full"
                >
                  <option>Alta</option>
                  <option>Media</option>
                  <option>Baja</option>
                </select>
              </div>

              <div class="flex flex-col gap-1.5">
                <label class="text-sm font-semibold text-zinc-700 dark:text-zinc-300">Estado</label>
                <select
                  v-model="form.status"
                  class="px-3 py-2.5 rounded-lg border border-zinc-300 dark:border-zinc-700 bg-white dark:bg-zinc-800 text-zinc-900 dark:text-zinc-100 focus:outline-none focus:ring-2 focus:ring-blue-500/20 transition-all text-sm w-full"
                >
                  <option>Pendiente</option>
                  <option>En proceso</option>
                  <option>Revisión</option>
                  <option>Finalizado</option>
                </select>
              </div>
            </div>

            <!-- Fecha finalización -->
            <div class="flex flex-col gap-1.5">
              <label class="text-sm font-semibold text-zinc-700 dark:text-zinc-300">Fecha de Finalización (Opcional)</label>
              <input
                v-model="form.finishedAt"
                type="date"
                class="px-3 py-2.5 rounded-lg border border-zinc-300 dark:border-zinc-700 bg-white dark:bg-zinc-800 text-zinc-900 dark:text-zinc-100 focus:outline-none focus:ring-2 focus:ring-blue-500/20 transition-all text-sm w-full"
              />
            </div>

            <!-- Botones -->
            <div class="flex flex-col sm:flex-row justify-end gap-3 pt-3 border-t border-zinc-100 dark:border-zinc-800 mt-1">
              <button
                type="button"
                @click="$emit('close')"
                class="w-full sm:w-auto px-4 py-2 rounded-lg text-sm font-medium text-zinc-700 dark:text-zinc-300 hover:bg-zinc-100 dark:hover:bg-zinc-800 transition-colors border border-zinc-200 dark:border-zinc-700"
              >
                Cancelar
              </button>
              <button
                type="submit"
                :disabled="loading"
                class="w-full sm:w-auto px-6 py-2 rounded-lg text-sm font-semibold bg-blue-600 text-white hover:bg-blue-700 disabled:opacity-60 transition-colors shadow-sm flex items-center justify-center gap-2"
              >
                <svg v-if="loading" class="animate-spin" xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <path d="M21 12a9 9 0 1 1-6.219-8.56"/>
                </svg>
                {{ loading ? 'Guardando...' : (mode === 'create' ? 'Crear Tarea' : 'Guardar Cambios') }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </Transition>
  </Teleport>
</template>

<script setup>
import { ref, watch } from 'vue'

const props = defineProps({
  isOpen: Boolean,
  mode: { type: String, default: 'create' },
  initialData: { type: Object, default: null },
  loading: { type: Boolean, default: false }
})

const emit = defineEmits(['close', 'submit'])

const defaultForm = () => ({
  title: '',
  description: '',
  priority: 'Media',
  status: 'Pendiente',
  finishedAt: ''
})

const form = ref(defaultForm())

watch(() => props.isOpen, (open) => {
  if (open) {
    if (props.initialData) {
      form.value = {
        title: props.initialData.title || '',
        description: props.initialData.description || '',
        priority: props.initialData.priority || 'Media',
        status: props.initialData.status || 'Pendiente',
        finishedAt: props.initialData.finishedAt
          ? new Date(props.initialData.finishedAt).toISOString().split('T')[0]
          : ''
      }
    } else {
      form.value = defaultForm()
    }
  }
})

function handleSubmit() {
  const payload = {
    ...form.value,
    finishedAt: form.value.finishedAt ? new Date(form.value.finishedAt).toISOString() : null
  }
  emit('submit', payload)
}
</script>

<style scoped>
.modal-enter-active, .modal-leave-active { transition: all 0.2s ease; }
.modal-enter-from, .modal-leave-to { opacity: 0; }
.modal-enter-from .relative, .modal-leave-to .relative { transform: scale(0.95) translateY(8px); }
</style>