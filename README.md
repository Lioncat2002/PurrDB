# PurrDB
A SpacetimeDB esque gameserver written iiin C# using SQLite and Quic Protocol

## Structure
The project contains 2 projects
- GameServer -> The actual game server running Quic Protocol and a in memory SQLite instance
- QuicClient -> A bare bones game client which will connect to the game server

## Steps to run
- Start the game server
- connect using the quic client
