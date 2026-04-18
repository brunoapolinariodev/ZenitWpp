import { Component, inject, signal, OnInit } from '@angular/core';
import { NgxEchartsDirective, provideEchartsCore } from 'ngx-echarts';
import * as echarts from 'echarts/core';
import type { EChartsOption } from 'echarts';
import { LineChart, BarChart } from 'echarts/charts';
import { TitleComponent, TooltipComponent, GridComponent, LegendComponent } from 'echarts/components';
import { CanvasRenderer } from 'echarts/renderers';
import { ReportService, DashboardResponse } from '../../../core/services/report.service';

echarts.use([LineChart, BarChart, TitleComponent, TooltipComponent, GridComponent, LegendComponent, CanvasRenderer]);

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [NgxEchartsDirective],
  providers: [provideEchartsCore({ echarts })],
  template: `
    <div class="flex-1 overflow-y-auto bg-slate-50 dark:bg-slate-900/40">
      <div class="max-w-5xl mx-auto px-8 py-8">

        <div class="mb-8">
          <h1 class="text-xl font-semibold text-slate-900 dark:text-slate-50">Relatórios</h1>
          <p class="text-sm text-slate-500 dark:text-slate-400 mt-1">Análise de desempenho e satisfação</p>
        </div>

        <!-- KPIs -->
        <div class="grid grid-cols-3 gap-4 mb-8">
          <div class="card p-5">
            <p class="text-xs font-medium text-slate-500 dark:text-slate-400 mb-1">Total hoje</p>
            <p class="text-3xl font-semibold text-slate-900 dark:text-slate-50">{{ data()?.totalToday ?? '—' }}</p>
            <p class="text-xs text-slate-400 dark:text-slate-500 mt-1">conversas atendidas</p>
          </div>
          <div class="card p-5">
            <p class="text-xs font-medium text-slate-500 dark:text-slate-400 mb-1">Tempo médio de resposta</p>
            <p class="text-3xl font-semibold text-slate-900 dark:text-slate-50">
              {{ data()?.avgResponseTimeMinutes ?? '—' }}<span class="text-lg font-normal text-slate-500 ml-1">min</span>
            </p>
            <p class="text-xs text-slate-400 dark:text-slate-500 mt-1">primeira resposta</p>
          </div>
          <div class="card p-5">
            <p class="text-xs font-medium text-slate-500 dark:text-slate-400 mb-1">Taxa de resolução</p>
            <p class="text-3xl font-semibold text-emerald-600 dark:text-emerald-400">87%</p>
            <p class="text-xs text-slate-400 dark:text-slate-500 mt-1">conversas encerradas</p>
          </div>
        </div>

        <!-- Gráficos -->
        <div class="grid grid-cols-1 gap-6">
          <div class="card p-5">
            <p class="text-sm font-medium text-slate-700 dark:text-slate-300 mb-4">Volume de conversas — últimos 7 dias</p>
            <div echarts [options]="lineOptions()" class="h-64"></div>
          </div>
          <div class="card p-5">
            <p class="text-sm font-medium text-slate-700 dark:text-slate-300 mb-4">Conversas por agente (hoje)</p>
            <div echarts [options]="agentBarOptions()" class="h-56"></div>
          </div>
        </div>

      </div>
    </div>
  `,
})
export class ReportsPageComponent implements OnInit {
  private svc = inject(ReportService);

  data        = signal<DashboardResponse | null>(null);
  lineOptions     = signal<EChartsOption>({});
  agentBarOptions = signal<EChartsOption>({});

  ngOnInit(): void {
    this.svc.getDashboard().subscribe({
      next:  d => { this.data.set(d); this.buildCharts(); },
      error: () => {
        this.data.set({ open: 12, inProgress: 7, waiting: 3, closed: 41, totalToday: 63, avgResponseTimeMinutes: 4 });
        this.buildCharts();
      },
    });
  }

  private buildCharts(): void {
    const days = ['Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'];
    this.lineOptions.set({
      tooltip: { trigger: 'axis' },
      legend: { data: ['Abertas', 'Encerradas'], bottom: 0, textStyle: { color: '#94a3b8', fontSize: 11 } },
      grid: { left: '3%', right: '3%', bottom: '12%', containLabel: true },
      xAxis: { type: 'category', data: days, axisLine: { show: false }, axisTick: { show: false }, axisLabel: { color: '#94a3b8', fontSize: 11 } },
      yAxis: { type: 'value', axisLine: { show: false }, axisTick: { show: false }, splitLine: { lineStyle: { color: '#f1f5f9' } }, axisLabel: { color: '#94a3b8', fontSize: 11 } },
      series: [
        { name: 'Abertas',    type: 'line', data: [22, 30, 25, 41, 36, 18, 12], smooth: true, lineStyle: { color: '#4f46e5', width: 2 }, itemStyle: { color: '#4f46e5' }, areaStyle: { color: 'rgba(79,70,229,0.06)' } },
        { name: 'Encerradas', type: 'line', data: [18, 27, 22, 38, 33, 15, 10], smooth: true, lineStyle: { color: '#10b981', width: 2 }, itemStyle: { color: '#10b981' }, areaStyle: { color: 'rgba(16,185,129,0.06)' } },
      ],
    });

    this.agentBarOptions.set({
      tooltip: { trigger: 'axis', axisPointer: { type: 'shadow' } },
      grid: { left: '3%', right: '3%', bottom: '3%', containLabel: true },
      xAxis: { type: 'value', axisLine: { show: false }, axisTick: { show: false }, splitLine: { lineStyle: { color: '#f1f5f9' } }, axisLabel: { color: '#94a3b8', fontSize: 11 } },
      yAxis: { type: 'category', data: ['Ana Lima', 'Carlos M.', 'Julia S.', 'Pedro R.', 'Sofia B.'], axisLine: { show: false }, axisTick: { show: false }, axisLabel: { color: '#64748b', fontSize: 11 } },
      series: [{
        type: 'bar', data: [14, 11, 9, 7, 5], barMaxWidth: 24,
        itemStyle: { color: '#4f46e5', borderRadius: [0, 4, 4, 0] },
        label: { show: true, position: 'right', color: '#64748b', fontSize: 11 },
      }],
    });
  }
}
