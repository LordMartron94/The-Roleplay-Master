use std::net::{TcpListener, TcpStream};
use std::io::{Read, Write};

pub struct Listener {
    pub(crate) ip: String,
    pub(crate) port: u16,
}

impl Listener {
    pub fn start(&self) -> Result<(), std::io::Error> {
        let address = format!("{}:{}", self.ip, self.port);
        let listener = TcpListener::bind(&address)?;

        println!("Listening on {}...", address);

        for stream in listener.incoming() {
            match stream {
                Ok(stream) => {
                    self.handle_connection(stream)?;
                }
                Err(e) => {
                    eprintln!("Error accepting connection: {}", e);
                }
            }
        }

        Ok(())
    }

    fn handle_connection(&self, mut stream: TcpStream) -> Result<(), std::io::Error> {
        let mut buffer = [0; 1024];
        let bytes_read = stream.read(&mut buffer)?;

        println!("Received {} bytes: {:?}", bytes_read, &buffer[..bytes_read]);

        // Process the received data and potentially send a response
        let response = "Hello from the Lifecycle Management Component!";
        stream.write(response.as_bytes())?;

        Ok(())
    }
}