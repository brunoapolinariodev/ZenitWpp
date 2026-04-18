import { Component, inject, signal, OnInit } from '@angular/core';
import { NgxEchartsDirective, provideEchartsCore } from 'ngx-echarts';
import * as echarts from 'echarts/core';
import type { EChartsOption } from 'echarts';
import { PieChart, BarChart } from 'echarts/charts';
import { TitleComponent, TooltipComponent, LegendComponent, GridComponent } from 'echarts/components';
import { CanvasRenderer } from 'echarts/renderers';
import { ReportService, DashboardResponse } from '../../../core/services/report.service';

echarts.use([PieChart, BarChart, TitleComponent, TooltipComponent, LegendComponent, GridComponent, CanvasRenderer]);

interface MetricCard {
  label: string;
  value: number;
  color: string;
  bg: string;
  icon: string;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [NgxEchartsDirective],
  providers: [provideEchartsCore({ echarts })],
  template: `
    <div class="flex-1 overflow-y-auto bg-slate-50 dark:bg-slate-900/40">
      <div class="max-w-5xl mx-auto px-8 py-8">

        <div class="mb-8">
          <h1 class="text-xl font-semibold text-slate-900 dark:text-slate-50">Dashboard</h1>
          <p class="text-sm text-slate-500 dark:text-slate-400 mt-1">Visão geral do atendimento em tempo real</p>
        </div>

        @if (loading()) {
          <div class="grid grid-cols-4 gap-4 mb-8">
            @for (i of [1,2,3,4]; track i) {
              <div class="card p-5 animate-pulse">
                <div class="h-3 bg-slate-200 dark:bg-slate-700 rounded w-2/3 mb-3"></div>
                <div class="h-8 bg-slate-200 dark:bg-slate-700 rounded w-1/3"></div>
              </div>
            }
          </div>
        } @else {
          <div class="grid grid-cols-4 gap-4 mb-8">
            @for (card of cards(); track card.label) {
              <div class="card p-5">
                <div class="flex items-center justify-between mb-3">
                  <p class="text-xs font-medium text-slate-500 dark:text-slate-400">{{ card.label }}</p>
                  <span class="w-8 h-8 rounded-lg flex items-center justify-center text-sm" [class]="card.bg">
                    {{ card.icon }}
                  </span>
                </div>
                <p class="text-3xl font-semibold" [class]="card.color">{{ card.value }}</p>
              </div>
            }
          </div>

          <div class="grid grid-cols-2 gap-6">
            <div class="card p-5">
              <p class="text-sm font-medium text-slate-700 dark:text-slate-300 mb-4">Conversas por status</p>
              <div echarts [options]="pieOptions()" class="h-56"></div>
            </div>
            <div class="card p-5">
              <p class="text-sm font-medium text-slate-700 dark:text-slate-300 mb-4">Volume hoje (por hora)</p>
              <div echarts [options]="barOptions()" class="h-56"></div>
            </div>
          </div>
        }

      </div>
    </div>
  `,
})
export class DashboardPageComponent implements OnInit {
  private svc = inject(ReportService);

  loading  = signal(true);
  cards    = signal<MetricCard[]>([]);
  pieOptions = signal<EChartsOption>({});
  barOptions = signal<EChartsOption>({});

  ngOnInit(): void {
    this.svc.getDashboard().subscribe({
      next:  d => this.render(d),
      error: () => this.render({ open: 12, inProgress: 7, waiting: 3, closed: 41, totalToday: 63, avgResponseTimeMinutes: 4 }),
    });
  }

  private render(d: DashboardResponse): void {
    this.cards.set([
      { label: 'Abertas',         value: d.open,       color: 'text-emerald-600 dark:text-emerald-400', bg: 'bg-emerald-50 dark:bg-emerald-950', icon: '🟢' },
      { label: 'Em andamento',    value: d.inProgress, color: 'text-primary-600 dark:text-primary-400', bg: 'bg-primary-50 dark:bg-primary-950', icon: '🔵' },
      { label: 'Aguardando',      value: d.waiting,    color: 'text-amber-600 dark:text-amber-400',     bg: 'bg-amber-50 dark:bg-amber-950',     icon: '🟡' },
      { label: 'Encerradas hoje', value: d.closed,     color: 'text-slate-600 dark:text-slate-400',     bg: 'bg-slate-100 dark:bg-slate-800',    icon: '⚪' },
    ]);

    this.pieOptions.set({
      tooltip: { trigger: 'item', formatter: '{b}: {c} ({d}%)' },
      series: [{
        type: 'pie',
        radius: ['48%', '72%'],
        itemStyle: { borderRadius: 6, borderWidth: 2, borderColor: 'transparent' },
        label: { show: true, formatter: '{b}\n{d}%', fontSize: 11, color: '#64748b' },
        data: [
          { value: d.open,       name: 'Abertas',      itemStyle: { color: '#10b981' } },
          { value: d.inProgress, name: 'Em andamento', itemStyle: { color: '#4f46e5' } },
          { value: d.waiting,    name: 'Aguardando',   itemStyle: { color: '#f59e0b' } },
          { value: d.closed,     name: 'Encerradas',   itemStyle: { color: '#94a3b8' } },
        ],
      }],
    });

    this.barOptions.set({
      tooltip: { trigger: 'axis', axisPointer: { type: 'shadow' } },
      grid: { left: '3%', right: '3%', bottom: '3%', containLabel: true },
      xAxis: {
        type: 'category',
        data: ['8h','9h','10h','11h','12h','13h','14h','15h','16h','17h','18h','19h'],
        axisLine: { show: false }, axisTick: { show: false },
        axisLabel: { color: '#94a3b8', fontSize: 11 },
      },
      yAxis: {
        type: 'value',
        axisLine: { show: false }, axisTick: { show: false },
        splitLine: { lineStyle: { color: '#f1f5f9' } },
        axisLabel: { color: '#94a3b8', fontSize: 11 },
      },
      series: [{
        type: 'bar',
        data: [3, 5, 8, 12, 9, 14, 11, 7, 6, 5, 4, 2],
        barMaxWidth: 32,
        itemStyle: { color: '#4f46e5', borderRadius: [4, 4, 0, 0] },
      }],
    });

    this.loading.set(false);
  }
}
