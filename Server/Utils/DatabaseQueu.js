class DatabaseQueu {
    constructor(db) {
        this.queu = [];
        this.running = false;
        this.db = db;
    }
    addFunction(func) {
        if (this.running == false) {
            var promise = new Promise(func);
            promise.then(this.nextQueue).catch((err) => {
                console.error(err)
            });
            this.running = true;
        } else {
            this.queu.push(func);
            console.log("queu length1: " + this.queu.length)
            console.log(this.queu)
        }
    }
    nextQueue(self) {
        console.log("queu length2: " + self.queu.length)
        if (self.queu.length > 0) {
            var promise = new Promise(self.queu[0]);
            self.queu.splice(0, 1);
            promise.then(self.nextQueue).catch((err) => {
                console.error(err)
                this.nextQueue()
            });
        } else {
            self.running = false;
        }
    }
}
module.exports = DatabaseQueu;