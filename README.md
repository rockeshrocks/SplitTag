# SplitTag
## Overview

SplitTag is a utility program designed to extract and generate individual process instrument tags in the KKS (Kraftwerk-Kennzeichen-System) format from compound tags.

## Example

Input Tag: `10HBK10-20CP501&502`
Output Tags:
- `10HBK10CP501`
- `10HBK10CP502`
- `10HBK20CP501`
- `10HBK20CP502`

## Typical Instrument Tag Format

A single instrument tag follows the format: `10HBK10CP501`.

## Features

* Automatically identifies and processes tag ranges (e.g., `10HBK10-20`)
* Handles multiple tag suffixes (e.g., `CP501&502`)
* Outputs all possible combinations of valid tags

## Usage

This program is ideal for applications in process control, instrumentation, or any scenario where KKS-format tags need to be processed and expanded for further analysis or documentation.
