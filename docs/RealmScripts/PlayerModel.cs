// :code-block-start: unity-player-model
// :state-start: final
// >>>> HERE
// :state-end: :state-uncomment-start: start
//// TODO: implement schema
// :state-uncomment-end:
// :code-block-end:



using System.Collections.Generic;
using MongoDB.Bson;
using Realms;

// :code-block-start: player-model
// :state-start: local sync
public class PlayerModel : RealmObject { 

    [PrimaryKey]
    [MapTo("_id")]
    [Required]
    public string Id { get; set; }

    [MapTo("_partition")]
    [Required]
    public string Partition { get; set; }

    [MapTo("scores")]
    public IList<ScoreModel> Scores { get; }

    [MapTo("name")]
    [Required]
    public string Name { get; set; }

}
// :state-end: :state-uncomment-start: start
// // TODO: Realm-ify Task model
// class Task {
//    var name: String = ""
//    var statusEnum: TaskStatus = .Open
// }
// :state-uncomment-end:
// :code-block-end:


// :code-block-start: task-model
// :state-start: local sync
class Task: Object {
    @Persisted(primaryKey: true) var _id: ObjectId
    @Persisted var name: String = ""
    @Persisted var owner: String?
    @Persisted var status: String = ""

    var statusEnum: TaskStatus {
        get {
            return TaskStatus(rawValue: status) ?? .Open
        }
        set {
            status = newValue.rawValue
        }
    }

    convenience init(name: String) {
        self.init()
        self.name = name
    }
}
// :state-end: :state-uncomment-start: start
// // TODO: Realm-ify Task model
// class Task {
//    var name: String = ""
//    var statusEnum: TaskStatus = .Open
// }
// :state-uncomment-end:
// :code-block-end: