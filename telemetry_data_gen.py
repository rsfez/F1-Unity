import fastf1 as ff1
import math
import numpy as np
import sys
import os


def to_ms(time):
    total_seconds = time.total_seconds()
    if not math.isnan(total_seconds):
        return round(total_seconds * 1000)
    else:
        return -1


def series_to_ms(series):
    return series.apply(lambda time: to_ms(time))


if __name__ == '__main__':
    np.set_printoptions(threshold=sys.maxsize)

    year = 2023
    gp = 'Japan'
    ses = 'R'

    session = ff1.get_session(year, gp, ses)
    weekend = session.event
    session.load()
    lap = session.laps.pick_fastest()
    pos = lap.get_pos_data()
    track = pos.loc[:, ('X', 'Y')].to_numpy()

    base_directory = os.path.join('Assets', 'Resources', 'Data', str(year), gp, ses)
    drivers_directory = os.path.join(base_directory, 'drivers')
    telemetry_directory = os.path.join(base_directory, 'telemetry')
    if not os.path.exists(base_directory):
        os.makedirs(base_directory)
    if not os.path.exists(drivers_directory):
        os.makedirs(drivers_directory)
    if not os.path.exists(telemetry_directory):
        os.makedirs(telemetry_directory)

    pos.to_csv(os.path.join(base_directory, 'track.csv'), columns=['X', 'Y'])
    laps = session.laps.copy()
    laps['Time'] = series_to_ms(laps['Time'])
    laps['LapTime'] = series_to_ms(laps['LapTime'])
    laps['Sector1Time'] = series_to_ms(laps['Sector1Time'])
    laps['Sector2Time'] = series_to_ms(laps['Sector2Time'])
    laps['Sector3Time'] = series_to_ms(laps['Sector3Time'])
    laps.to_csv(os.path.join(base_directory, 'session.csv'))

    for driver_id in session.drivers:
        driver = session.get_driver(driver_id)
        abbreviation = driver.Abbreviation
        driver.to_csv(os.path.join(drivers_directory, f'{abbreviation}.csv'))
        telemetry = session.laps.pick_driver(driver_id).get_telemetry()
        telemetry['Time'] = series_to_ms(telemetry['Time'])
        telemetry.to_csv(os.path.join(telemetry_directory, f'{abbreviation}.csv'),
                         columns=['Time', 'RPM', 'Speed', 'nGear', 'Throttle', 'Brake', 'DRS', 'X', 'Y', 'DriverAhead'])
