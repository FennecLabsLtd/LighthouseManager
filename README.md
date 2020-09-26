# Lighthouse Manager

[![Build status](https://ci.appveyor.com/api/projects/status/ekw2gnwcdhjaphjh?svg=true)](https://ci.appveyor.com/project/FennecLabs/lighthousemanager)

Lighthouse Manager is a quick and easy way to save/restore your Lighthouse and Chaperone configs. We recommend using it from a shared network drive or USB stick so that you can very quickly open it, restore your room setup and move on to the next PC.

You can download the latest build [here](https://github.com/FennecLabsLtd/LighthouseManager/releases/latest)

## Usage

It's so ridiculously easy to use, just run the EXE and the rest is self explanatory 

### Command Line Usage

| Short Version | Long Version | Description |
| -------------- | ------------- | ----------- |
| `-s [filename]` | `--save [filename]` | Specifies the map file to save to (with or without the .rcfg extension, it will be added automatically) |
| `-l [filename]` | `--load [filename]` | Specifies the file load from (again, with or without the .rcfg extension) |
| `-r` | `--restart` | Stops SteamVR before running any save/load (generally only needed when loading), and starts it again when finished |
| `-?`| `--help` | Displays the help text |

## Contributing

Contributions are always welcome, just submit a PR!

## Disclaimer

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
