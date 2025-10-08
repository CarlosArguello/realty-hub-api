#!/bin/sh
dotnet Api.dll --seed
exec dotnet Api.dll
