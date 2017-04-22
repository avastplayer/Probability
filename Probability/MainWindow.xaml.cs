using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Visifire.Charts;

namespace Probability
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            int times = 1000000;
            int[] weightArray = new int[60];
            int max = 0;
            int max2 = 0;
            
            for (int i = 0;i< weightArray.Length;i++)
            {
                int k = (i % 10 == 1) ? 8 : 1;
                
                weightArray[i] = (int) Math.Pow(10, k);
                
                max = max + weightArray[i];
            }
            var result = Probability(weightArray, times, max);

            foreach (int t in result)
            {
                max2 = max2 + t;
            }


            double[] prob1 = new double[60];
            double[] prob2 = new double[60];
            int[] num = new int[60];

            for (int i = 0; i < weightArray.Length; i++)
            {
                num[i] = i + 1;
                prob1[i] = (double)weightArray[i] / (double)max;
                prob2[i] = (double)result[i] / (double)max2;

            }
            
            CreateChartSpline("概率模拟", num, prob1, prob2);
        }

        public int[] Probability(int[] weightArray, int times, int max)
        {
            int[] arr = new int[60];
            int[] probability = new int[60];
            var number = new List<int>();
            for (int i = 0; i < weightArray.Length-1; i++)
            {
                arr[i + 1] = arr[i] + weightArray[i];
            }
            while (times > 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    Random random = new Random(unchecked((int)DateTime.Now.Ticks));
                    int index = random.Next(1, max);
                    for (int j = 0; j < arr.Length; j++)
                    {
                        if (index >= arr[j]) continue;
                        if (number.Contains(j - 1)) continue;
                        number.Add(j - 1);
                        break;
                    }

                }
                foreach (int t in number)
                {
                    probability[t]++;
                }
                number.Clear();
                times--;
            }

            return probability;
        }

        public void CreateChartSpline(string name, int[] num, double[] prob1, double[] prob2)
        {
            //创建一个图标
            Chart chart = new Chart
            {
                Width = 580,
                Height = 380,
                Margin = new Thickness(100, 5, 10, 5),
                ToolBarEnabled = false,
                ScrollingEnabled = false,
                View3D = true
            };

            //设置图标的宽度和高度
            //是否启用打印和保持图片

            //设置图标的属性
            //是否启用或禁用滚动
            //3D效果显示

            //创建一个标题的对象
            Title title = new Title
            {
                Text = name,
                Padding = new Thickness(0, 10, 5, 0)
            };

            //设置标题的名称

            //向图标添加标题
            chart.Titles.Add(title);

            //初始化一个新的Axis
            Axis xaxis = new Axis();
            
            //设置Axis的属性
            //图表的X轴坐标按什么来分类，如时分秒
            //xaxis.IntervalType = IntervalTypes.Auto;
            //图表的X轴坐标间隔如2,3,20等，单位为xAxis.IntervalType设置的时分秒。
            //xaxis.Interval = 1;
            //设置X轴的时间显示格式为7-10 11：20           
            //xaxis.ValueFormatString = "MM月";
            //给图标添加Axis            
            chart.AxesX.Add(xaxis);

            Axis yAxis = new Axis();
            //设置图标中Y轴的最小值永远为0           
            yAxis.AxisMinimum = 0;
            //设置图表中Y轴的后缀          
            //yAxis.Suffix = "斤";
            chart.AxesY.Add(yAxis);


            // 创建一个新的数据线。               
            DataSeries dataSeries = new DataSeries();
            // 设置数据线的格式。               
            dataSeries.LegendText = "初始概率";

            dataSeries.RenderAs = RenderAs.Spline;//折线图

            dataSeries.XValueType = ChartValueTypes.Auto;
            // 设置数据点              
            DataPoint dataPoint;
            for (int i = 0; i < num.Length; i++)
            {
                // 创建一个数据点的实例。                   
                dataPoint = new DataPoint();
                // 设置X轴点                    
                dataPoint.XValue = num[i];
                //设置Y轴点                   
                dataPoint.YValue = prob1[i];
                dataPoint.MarkerSize = 8;
                //dataPoint.Tag = tableName.Split('(')[0];
                //设置数据点颜色                  
                // dataPoint.Color = new SolidColorBrush(Colors.LightGray);                   
                //dataPoint.MouseLeftButtonDown += new MouseButtonEventHandler(dataPoint_MouseLeftButtonDown);
                //添加数据点                   
                dataSeries.DataPoints.Add(dataPoint);
            }

            // 添加数据线到数据序列。                
            chart.Series.Add(dataSeries);


            // 创建一个新的数据线。               
            DataSeries dataSeriesPineapple = new DataSeries();
            // 设置数据线的格式。         

            dataSeriesPineapple.LegendText = "抽取概率";

            dataSeriesPineapple.RenderAs = RenderAs.Spline;//折线图

            dataSeriesPineapple.XValueType = ChartValueTypes.Auto;
            // 设置数据点              

            DataPoint dataPoint2;
            for (int i = 0; i < num.Length; i++)
            {
                // 创建一个数据点的实例。                   
                dataPoint2 = new DataPoint();
                // 设置X轴点                    
                dataPoint2.XValue = num[i];
                //设置Y轴点                   
                dataPoint2.YValue = prob2[i];
                dataPoint2.MarkerSize = 8;
                //dataPoint2.Tag = tableName.Split('(')[0];
                //设置数据点颜色                  
                // dataPoint.Color = new SolidColorBrush(Colors.LightGray);                   
                //dataPoint2.MouseLeftButtonDown += new MouseButtonEventHandler(dataPoint_MouseLeftButtonDown);
                //添加数据点                   
                dataSeriesPineapple.DataPoints.Add(dataPoint2);
            }
            // 添加数据线到数据序列。                
            chart.Series.Add(dataSeriesPineapple);

            //将生产的图表增加到Grid，然后通过Grid添加到上层Grid.           
            Grid gr = new Grid();
            gr.Children.Add(chart);
            Simon.Children.Add(gr);
        }
    }
}
