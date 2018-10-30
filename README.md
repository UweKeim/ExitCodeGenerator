# Exit Code Generator

Tool to sleep for a given amount of milliseconds and then return a given exit code.

**[Download latest executable](https://github.com/UweKeim/ExitCodeGenerator/releases/download/v1.0.0.0/exitcode-generator.exe)**

## Introduction

This is a small utility to sleep for a given amount of milliseconds and then return a given exit code. It is intended as a mockup for testing external application calls.

## Usage

	exitcode-generator.exe 
	    [--exitcode=<code>] 
	    [--sleep=<milliseconds>] 
	    [--log=<true|false>] 
	    [--keeplogfiles=<true|false>] 
	    [--quiet=<true|false>]

(Multi-line for illustration purposes only; call as a single line in real life).

## Command line arguments

`--exitcode=<code>`:
> Optional. Specify an integer exit code, positive or negative or zero to
> return from the application. If none is specified, zero is returned.

`--sleep=<milliseconds>`:
> Optional. Specify a positive amount of milliseconds to sleep before exiting.
> If none is specified, the application returns immediately.

`--log=<true|false>`:
> Optional. Specify whether to log to a file at all.
> If not specified, the default value is false.

`--keeplogfile=<true|false>`:
> Optional. Specify whether to clear any existing log file upon program start.
> If not specified, the default value is true. Useful for calling the program
> multiple times and having one large cumulated log file.

`--quiet=<true|false>`:
> Optional. Specify whether to output anything to the console at all.
> If not specified, the default value is false, meaning it does output text.

## Examples

Immediately return exit code -2:

>     exitcode-generator.exe --exitcode=-2

Wait for 2 seconds and then return exit code 5:

>     exitcode-generator.exe --sleep=2000 --exitcode=5

Wait for 3.5 seconds and then return exit code 0:

>     exitcode-generator.exe --sleep=3500

