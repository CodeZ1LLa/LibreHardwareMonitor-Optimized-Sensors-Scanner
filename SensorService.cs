//using OpenHardwareMonitor.Hardware;
using LibreHardwareMonitor.Hardware;

namespace WindowsFormsApp3
{
    public class SensorService
    {
        public SensorService()
        {
            computer = new Computer()
            {
                IsCpuEnabled = true,
            };
            computer.Open();
        }

        public float BusSpeed { get; private set; }
        public float CCX1Freq { get; private set; }
        public float CCX1Temp { get; private set; }
        public float CCX2Temp { get; private set; }
        public float Core1F { get; private set; }
        public float CoreFrequency { get; private set; }
        public float CpuFrequency { get; private set; }
        public float CPUFrequency { get; private set; }
        public float CpuPPT { get; private set; }
        public float CpuTemp { get; private set; }
        public float CpuUsage { get; private set; }
        public float VCoreVoltage { get; private set; }

        public void ReportSensors()
        {
            for (int i = 0; i < computer.Hardware.Count; i++)
            {
                IHardware hardware = computer.Hardware[i];
                if (hardware.HardwareType == HardwareType.Cpu)
                {
                    // only fire the update when found
                    hardware.Update();

                    // loop through the data
                    for (int i1 = 0; i1 < hardware.Sensors.Length; i1++)
                    {
                        ISensor sensor = hardware.Sensors[i1];
                        switch (sensor.SensorType)
                        {
                            case SensorType.Voltage when sensor.Name.Contains("Core (SVI2 TFN)"):
                                VCoreVoltage = sensor.Value.GetValueOrDefault();
                                break;

                            case SensorType.Clock when sensor.Name.Contains("Core #" + OC_MATE_UI.coreCounter):
                                CoreFrequency = sensor.Value.GetValueOrDefault();
                                break;

                            case SensorType.Temperature when sensor.Name.Contains("Core (Tctl/Tdie)"):
                                CpuTemp = sensor.Value.GetValueOrDefault();
                                break;

                            case SensorType.Load when sensor.Name.Contains("CPU Total"):
                                CpuUsage = sensor.Value.GetValueOrDefault();
                                break;

                            case SensorType.Power when sensor.Name.Contains("Package"):
                                CpuPPT = sensor.Value.GetValueOrDefault();
                                break;

                            case SensorType.Clock when sensor.Name.Contains("CPU"):
                                CPUFrequency = sensor.Value.GetValueOrDefault();
                                break;

                            case SensorType.Temperature when sensor.Name.Contains("CCD1 (Tdie)"):
                                CCX1Temp = sensor.Value.GetValueOrDefault();
                                break;

                            case SensorType.Temperature when sensor.Name.Contains("CCD2 (Tdie)"):
                                CCX2Temp = (int)sensor.Value.GetValueOrDefault();
                                break;
                        }
                    }
                }
            }
        }

        private Computer computer;
    }
}