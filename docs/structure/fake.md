# [Enms.Fake](scripts/Enms.Fake)

This script project runs in a loop and sends faked measurements to the server.
Here is an outline of the various options that can be passed to the project to
customize its behaviour:

| Option     | Description                    | Example         | Explanation                             |
| ---------- | ------------------------------ | --------------- | --------------------------------------- |
| --interval | Time between each request      | --interval 1000 | Send a request each second.             |
| ---------- | ------------------------------ | --------------- | --------------------------------------- |
| --amount   | Amount of measurements to send | --amount 1000   | Send a thousand requests each interval. |
