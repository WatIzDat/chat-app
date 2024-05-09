import DiscussionLink from "@/app/ui/discussion-link";
import { fetchJoinedDiscussionsByUser } from "@/app/lib/data";
import { auth } from "@clerk/nextjs/server";

export default async function Page() {
    const discussions: { id: string; name: string }[] =
        await fetchJoinedDiscussionsByUser(
            auth().sessionClaims?.userId as string
        );

    const discussionLinks = [];

    for (let i = 0; i < discussions.length; i++) {
        discussionLinks.push(
            <DiscussionLink
                id={discussions[i].id}
                name={discussions[i].name}
                key={i}
            />
        );
    }

    return (
        <main className="h-full">
            <div className="flex flex-wrap gap-8 items-center justify-center h-full">
                {discussionLinks}
            </div>
        </main>
    );
}
