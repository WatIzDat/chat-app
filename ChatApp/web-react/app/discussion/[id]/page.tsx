import { fetchJoinedDiscussionsByUser } from "@/app/lib/data";
import SendMessageForm from "@/app/ui/send-message-form";
import { redirect } from "next/navigation";

export default async function Page({ params }: { params: { id: string } }) {
    const joinedDiscussions: { id: string; name: string }[] =
        await fetchJoinedDiscussionsByUser();

    if (
        !joinedDiscussions ||
        !joinedDiscussions.map((d) => d.id).includes(params.id)
    ) {
        return redirect(`/join-discussion/${params.id}`);
    }

    return (
        <main className="h-full">
            <div className="flex flex-col items-center justify-end h-full">
                <SendMessageForm discussionId={params.id} />
            </div>
        </main>
    );
}
