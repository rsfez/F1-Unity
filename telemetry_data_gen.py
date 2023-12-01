import fastf1 as ff1
import numpy as np
import sys
import os

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
    drivers_directory = os.path.join(ses_directory, 'drivers')
    telemetry_directory = os.path.join(ses_directory, 'telemetry')
    if not os.path.exists(ses_directory):
        os.makedirs(ses_directory)
    if not os.path.exists(drivers_directory):
        os.makedirs(drivers_directory)
    if not os.path.exists(telemetry_directory):
        os.makedirs(telemetry_directory)

    pos.to_csv(os.path.join(ses_directory, 'track.csv'), columns=['X', 'Y'])
    session.laps.to_csv(os.path.join(ses_directory, 'session.csv'))

    for driver_id in session.drivers:
        driver = session.get_driver(driver_id)
        abbreviation = driver.Abbreviation
        driver.to_csv(os.path.join(drivers_directory, f'{abbreviation}.csv'))
        telemetry = session.laps.pick_driver(driver_id).get_telemetry()
        telemetry['Time'] = telemetry['Time'].apply(lambda time: (int(round(time.total_seconds() * 1000))))
        telemetry.to_csv(os.path.join(telemetry_directory, f'{abbreviation}.csv'), columns=['Time', 'RPM', 'Speed', 'nGear', 'Throttle', 'Brake', 'DRS', 'X', 'Y', 'DriverAhead'])
        telemetry.to_csv(os.path.join(telemetry_directory, f'{abbreviation}.csv'))
