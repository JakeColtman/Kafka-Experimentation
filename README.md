# Kafka-Sketch

I want to implement Kafka stream at work to facilitate a reporting service that feeds off multiple different services across the company.  This is a simple draft of how this could look.  This is not at all designed to be a real system, in practice it's much better to sink the updates to a file and bulk upload and such and such but it serves as a fun sketch!

Things it supports:

* Adding simple company updates
* Stream company updates for real time actions
* Periodically updates the SQL Server db with new updates for long term storage

Things to flesh out in the real version:

* More efficient dumping
* Cancellation tokens for the one off storing
* Serialization/Deserialization
