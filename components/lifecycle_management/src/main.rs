mod communication;

use communication::Listener;

fn main() {
    // Create a Listener instance
    let listener = Listener {
        ip: "127.0.0.1".to_string(),
        port: 8000,
    };

    // Start the listener
    match listener.start() {
        Ok(_) => println!("Server exited normally."),
        Err(e) => eprintln!("Server exited due to an error: {}", e),
    }
}