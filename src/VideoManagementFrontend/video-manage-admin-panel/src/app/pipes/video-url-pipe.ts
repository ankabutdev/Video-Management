import { Pipe, PipeTransform } from "@angular/core";
import { environment } from "../../environments/environment.prod";

@Pipe({
    name: 'videoUrl',
    standalone: true
})

export class VideoUrlPipe implements PipeTransform {
    transform(value: string): string {
        return `${environment.apiUrl}\\${value}`
    }
}