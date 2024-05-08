import { joinDiscussion } from "@/app/lib/actions";
import { fetchDiscussionById } from "@/app/lib/data";

export default async function Page({ params }: { params: { id: string } }) {
    const discussion: { id: string; name: string } | undefined =
        await fetchDiscussionById(params.id);

    let joinDiscussionWithId;

    if (discussion) {
        joinDiscussionWithId = joinDiscussion.bind(null, discussion.id);
    }

    return (
        <main className="h-full">
            <div className="flex items-center justify-center h-full">
                {discussion && (
                    <form
                        className="flex flex-col"
                        action={joinDiscussionWithId}
                    >
                        <h3 className="text-2xl font-bold">
                            Join {discussion.name}?
                        </h3>

                        <button
                            className="bg-zinc-700 p-2 rounded mt-4"
                            type="submit"
                        >
                            Join
                        </button>
                    </form>
                )}
                {!discussion && (
                    <h3 className="text-2xl font-bold">
                        Could not find this discussion :(
                    </h3>
                )}
            </div>
        </main>
    );
}
