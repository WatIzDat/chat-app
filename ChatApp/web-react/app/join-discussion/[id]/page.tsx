import { joinDiscussion } from "@/app/lib/actions";
import {
    fetchDiscussionById,
    fetchJoinedDiscussionsByUser,
} from "@/app/lib/data";
import { auth } from "@clerk/nextjs/server";
import { redirect } from "next/navigation";

export default async function Page({ params }: { params: { id: string } }) {
    const joinedDiscussions: { id: string; name: string }[] =
        await fetchJoinedDiscussionsByUser(
            auth().sessionClaims?.userId as string
        );

    if (
        joinedDiscussions &&
        joinedDiscussions.map((d) => d.id).includes(params.id)
    ) {
        return redirect(`/discussion/${params.id}`);
    }

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
